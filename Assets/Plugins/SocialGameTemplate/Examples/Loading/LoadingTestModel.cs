using System;
using SocialGame.Loading;
using UnityEngine;
using Zenject;
using UniRx;

namespace Sandbox.Loading
{
    public sealed class LoadingTestModel : IInitializable, IDisposable
    {
        [Inject] private LoadingController _controller = null;

        private readonly CompositeDisposable _disposable = new CompositeDisposable();

        void IInitializable.Initialize()
        {
            Observable
                .EveryUpdate()
                .Where(_ => Input.GetKeyDown(KeyCode.S))
                .Subscribe(_ => _controller.Show(SocialGame.Loading.LoadingType.Sample))
                .AddTo(_disposable);

            Observable
                .EveryUpdate()
                .Where(_ => Input.GetKeyDown(KeyCode.H))
                .Subscribe(_ => _controller.Hide())
                .AddTo(_disposable);
        }

        void IDisposable.Dispose()
        {
            _disposable.Dispose();
        }
    }
}