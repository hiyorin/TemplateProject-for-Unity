using System;
using UnityEngine;
using SocialGame.TapEffect;
using Zenject;
using UniRx;

namespace Sandbox.TapEffect
{
    public sealed class TapEffectTestModel : IInitializable, IDisposable
    {
        [Inject] private TapEffectController _controller = null;

        private readonly CompositeDisposable _disposable = new CompositeDisposable();

        void IInitializable.Initialize()
        {
            Observable
                .EveryUpdate()
                .Where(_ => Input.GetKeyDown(KeyCode.S))
                .Subscribe(_ => _controller.Start(SocialGame.TapEffect.TapEffectType.Sample))
                .AddTo(_disposable);

            Observable
                .EveryUpdate()
                .Where(_ => Input.GetKeyDown(KeyCode.H))
                .Subscribe(_ => _controller.Stop())
                .AddTo(_disposable);
        }

        void IDisposable.Dispose()
        {
            _disposable.Dispose();
        }
    }
}