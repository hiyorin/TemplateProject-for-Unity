using System;
using UnityEngine;
using SocialGame.Data;
using Zenject;
using UniRx;

namespace Sandbox.Resolution
{
    public sealed class ResolutionTestModel : IInitializable, IDisposable
    {
        [Inject] private IResolutionController _controller;

        private readonly CompositeDisposable _disposable = new CompositeDisposable();
        
        void IInitializable.Initialize()
        {
            var hight = Observable.EveryUpdate()
                .Where(_ => Input.GetKeyDown(KeyCode.A))
                .Select(_ => Quality.High)
                .Publish()
                .RefCount();

            var mid = Observable.EveryUpdate()
                .Where(_ => Input.GetKeyDown(KeyCode.S))
                .Select(_ => Quality.Standard)
                .Publish()
                .RefCount();
            
            var low = Observable.EveryUpdate()
                .Where(_ => Input.GetKeyDown(KeyCode.D))
                .Select(_ => Quality.Low)
                .Publish()
                .RefCount();

            Observable.Merge(hight, mid, low)
                .SelectMany(x => _controller.Put(x))
                .Subscribe(x =>
                {
                    Debug.unityLogger.Log(GetType().Name, Screen.width + "x" + Screen.height);
                })
                .AddTo(_disposable);
        }

        void IDisposable.Dispose()
        {
            _disposable.Dispose();
        }
    }
}
