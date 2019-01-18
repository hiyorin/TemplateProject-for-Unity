using System;
using SocialGame.Scene;
using SocialGame.Transition;
using UnityEngine;
using Zenject;
using UniRx;
using UniRx.Async;

namespace Sandbox.Scene
{
    public class SceneAModel : ISceneLifecycle, IInitializable, IDisposable
    {
        [Inject] private ISceneManager _sceneManager = null;

        private readonly CompositeDisposable _disposable = new CompositeDisposable();

        public void Initialize()
        {
            
        }

        public void Dispose()
        {
            _disposable.Dispose();
        }

        async UniTask ISceneLifecycle.OnLoad(object transData)
        {
            Debug.unityLogger.Log(GetType().Name, "OnLoad " + transData);
        }

        async UniTask ISceneLifecycle.OnTransIn()
        {
            Debug.unityLogger.Log(GetType().Name, "OnTransIn");
        }

        void ISceneLifecycle.OnTransComplete()
        {
            Debug.unityLogger.Log(GetType().Name, "OnTransComplete");
            Observable.EveryUpdate()
                .Where(_ => Input.GetKeyDown(KeyCode.Mouse0))
                .Subscribe(_ => _sceneManager.Next("SceneB", "prev SceneA", TransMode.BlackFade))
                .AddTo(_disposable);
            Observable.EveryUpdate()
                .Where(_ => Input.GetKeyDown(KeyCode.R))
                .Subscribe(_ => _sceneManager.Reload())
                .AddTo(_disposable);
        }

        async UniTask ISceneLifecycle.OnTransOut()
        {
            Debug.unityLogger.Log(GetType().Name, "OnTransOut");
        }

        async UniTask ISceneLifecycle.OnUnload()
        {
            Debug.unityLogger.Log(GetType().Name, "OnUnload");
        }
    }
}
