using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using SocialGame.Transition;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityExtensions;
using Zenject;
using UniRx;
using UnitySceneManager = UnityEngine.SceneManagement.SceneManager;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace SocialGame.Scene
{
    public sealed class SceneManager : MonoBehaviour, ISceneManager
    {
        private class History
        {
            public string SceneName;
            public object TransData;
        }

        private static bool IsLoading;

        [Inject] private TransitionController _transController;

        [Inject] private EventSystem _eventSystem;

        private LoadContext _loadContext;

        private readonly Stack<History> _histories = new Stack<History>();

        private IEnumerator Start()
        {
            var scene = UnitySceneManager.GetActiveScene();
            var context = FindSceneContext(scene.name);
            yield return new WaitUntil(() => context.Initialized);

            /*
            _loadContext = new LoadContext() {
                NextScene = new SceneData() {
                    Name = scene.name,
                    Lifecycles = context.Container.ResolveAll<ISceneLifecycle>(),
                },
                AdditiveScenes = new List<SceneData>(),
            };
            _loadContext.NextScene.Lifecycles.ForEach(x => x.OnTransComplete());
            */
            Next(scene.name, null, TransMode.None);
        }

        private void Next(string sceneName, object transData, TransMode transMode)
        {
            if (IsLoading)
            {
                Debug.unityLogger.LogWarning(GetType().Name, "Loading");
                return;
            }

            if (_histories.Count > 0 && _histories.Peek().SceneName == sceneName)
            {
                Reload();
                return;
            }
            
            IsLoading = true;

            _histories.Push(new History() { SceneName = sceneName, TransData = transData });
            var nextContext = new LoadContext(sceneName, transData, transMode);
            StartCoroutine(Load(nextContext, _loadContext));
            _loadContext = nextContext;
        }

        private void Back()
        {
            if (IsLoading)
            {
                Debug.unityLogger.LogWarning(GetType().Name, "Loading");
                return;
            }

            if (_histories.Count < 2)
            {
                Debug.unityLogger.LogWarning(GetType().Name, "Empty history");
                return;
            }

            _histories.Pop();
            var history = _histories.Peek();
            var nextContext = new LoadContext(history.SceneName, history.TransData, TransMode.None);
            StartCoroutine(Load(nextContext, _loadContext));
            _loadContext = nextContext;
        }

        private void Reload()
        {
            if (IsLoading)
            {
                Debug.unityLogger.LogWarning(GetType().Name, "Loading");
                return;
            }
            
            IsLoading = true;
            StartCoroutine(Reload(_loadContext));
        }

        private IEnumerator Reload(LoadContext context)
        {
            _eventSystem.enabled = false;
            yield return context.TransOut().StartAsCoroutine();
            yield return _transController.TransIn(context.TransMode);
            yield return context.Unload().StartAsCoroutine();
            yield return context.Load().StartAsCoroutine();
            yield return _transController.TransOut();
            yield return context.TransIn().StartAsCoroutine();
            context.TransComplate();
            IsLoading = false;
            _eventSystem.enabled = true;
        }
        
        private IEnumerator Load(LoadContext next, LoadContext prev)
        {
            _eventSystem.enabled = false;
            if (prev != null) yield return prev.TransOut(next).StartAsCoroutine();
            yield return _transController.TransIn(next.TransMode);
            yield return LoadInternal(next);
            if (prev != null) yield return prev.Unload(next).StartAsCoroutine();
            if (prev != null) yield return UnloadInternal(prev, next);
            yield return LoadSubsInternal(next, prev);
            yield return next.Load(prev).StartAsCoroutine();
            yield return _transController.TransOut();
            yield return next.TransIn(prev).StartAsCoroutine();
            next.TransComplate();
            IsLoading = false;
            _eventSystem.enabled = true;
        }

        private static IEnumerator LoadInternal(LoadContext context)
        {
            if (context == null)
                yield break;
            
            var scene = UnitySceneManager.GetSceneByName(context.NextScene.Name);
            if (!scene.isLoaded)
                yield return UnitySceneManager.LoadSceneAsync(context.NextScene.Name, LoadSceneMode.Additive);
            UnitySceneManager.SetActiveScene(UnitySceneManager.GetSceneByName(context.NextScene.Name));

            var sceneContext = FindSceneContext(context.NextScene.Name);
            if (sceneContext == null)
                yield break;
            
            yield return new WaitUntil(() => sceneContext.Initialized);
            var sceneSettings = sceneContext.Container.TryResolve<SceneSettings>();
            if (sceneSettings != null)
                sceneSettings.Subs.ForEach(x => context.AddAdditiveScene(x));
            context.NextScene.Lifecycles = sceneContext.Container.ResolveAll<ISceneLifecycle>();
        }

        private static IEnumerator LoadSubsInternal(LoadContext context, LoadContext prev)
        {
            yield return context.AdditiveScenes
                .Where(x => {
                    if (prev == null) return true;
                    var cache = prev.AdditiveScenes.FirstOrDefault(y => x.Name == y.Name);
                    if (cache == null)
                        return true;
                    else
                        x.Lifecycles = cache.Lifecycles;
                    return false;
                })
                .Select(additiveScene => UnitySceneManager.LoadSceneAsync(additiveScene.Name, LoadSceneMode.Additive)
                    .AsObservable()
                    .Select(_ => FindSceneContext(additiveScene.Name))
                    .SelectMany(x => new WaitUntil(() => x.Initialized)
                        .ToObservable()
                        .Select(_ => x))
                    .Do(x => additiveScene.Lifecycles = x.Container.ResolveAll<ISceneLifecycle>().Where(y => !context.NextScene.Lifecycles.Any(z => y == z)))
                    .FirstOrDefault())
                .WhenAll()
                .StartAsCoroutine();

            Resources.UnloadUnusedAssets();

            GC.Collect();
        }

        private static IEnumerator UnloadInternal(LoadContext context, LoadContext next)
        {
            if (context == null)
                yield break;
            
            yield return context.AdditiveScenes
                .Where(x => next == null || !next.AdditiveScenes.Any(y => x.Name == y.Name))
                .Select(x => UnitySceneManager.UnloadSceneAsync(x.Name)
                    .ObserveEveryValueChanged(y => y.isDone)
                    .FirstOrDefault())
                .WhenAll()
                .StartAsCoroutine();

            yield return UnitySceneManager.UnloadSceneAsync(context.NextScene.Name);
        }

        private static SceneContext FindSceneContext(string sceneName)
        {
            var scene = UnitySceneManager.GetSceneByName(sceneName);
            foreach (var rootObject in scene.GetRootGameObjects())
            {
                var context = rootObject.GetComponent<SceneContext>();
                if (context != null)
                    return context;
            }
            return null;
        }

        #region ISceneManager implementation
        void ISceneManager.Next(Scene scene)
        {
            Next(scene.ToString(), null, TransMode.None);
        }

        void ISceneManager.Next(Scene scene, object transData, TransMode transMode)
        {
            Next(scene.ToString(), transData, transMode);
        }

        void ISceneManager.Next(string sceneName)
        {
            Next(sceneName, null, TransMode.None);
        }

        void ISceneManager.Next(string sceneName, object transData, TransMode transMode)
        {
            Next(sceneName, transData, transMode);
        }

        void ISceneManager.Back()
        {
            Back();
        }

        void ISceneManager.Reload()
        {
            Reload();
        }
        #endregion

        #if UNITY_EDITOR
        [CustomEditor(typeof(SceneManager))]
        private class CustomInspector : Editor
        {
            private SceneManager _owner;

            private void OnEnable()
            {
                _owner = target as SceneManager;
            }

            public override void OnInspectorGUI()
            {
                EditorGUILayout.LabelField("Histories");
                EditorGUI.indentLevel++;
                _owner._histories.Reverse().ForEach(x => {
                    EditorGUILayout.LabelField(x.SceneName);
                });
                EditorGUI.indentLevel--;
            }
        }
        #endif
    }
}
