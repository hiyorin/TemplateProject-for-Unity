using System;
using SocialGame.Scene;
using SocialGame.Transition;
using UnityEngine;
using Zenject;
using UniRx;

namespace Sandbox.Scene
{
    public class SceneBModel : ISceneLifecycle, IInitializable, IDisposable
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

        IObservable<Unit> ISceneLifecycle.OnLoad(object transData)
        {
            return Observable.ReturnUnit()
                .Do(_ => Debug.unityLogger.Log(GetType().Name, "OnLoad " + transData));
        }

        IObservable<Unit> ISceneLifecycle.OnTransIn()
        {
            return Observable.ReturnUnit()
                .Do(_ => Debug.unityLogger.Log(GetType().Name, "OnTransIn"));
        }

        void ISceneLifecycle.OnTransComplete()
        {
            Debug.unityLogger.Log(GetType().Name, "OnTransComplete");
            Observable.EveryUpdate()
                .Where(_ => Input.GetKeyDown(KeyCode.Mouse0))
                .Subscribe(_ => _sceneManager.Next("SceneA", "prev SceneB", TransMode.BlackFade))
                .AddTo(_disposable);
        }

        IObservable<Unit> ISceneLifecycle.OnTransOut()
        {
            return Observable.ReturnUnit()
                .Do(_ => Debug.unityLogger.Log(GetType().Name, "OnTransOut"));
        }

        IObservable<Unit> ISceneLifecycle.OnUnload()
        {
            return Observable.ReturnUnit()
                .Do(_ => Debug.unityLogger.Log(GetType().Name, "OnUnload"));
        }
    }
}
