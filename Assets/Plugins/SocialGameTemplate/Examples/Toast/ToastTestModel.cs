using System;
using UnityEngine;
using SocialGame.Toast;
using Zenject;
using UniRx;

namespace Sandbox.Toast
{
    public sealed class ToastTestModel : IInitializable, IDisposable
    {
        [Inject] private ToastController _controller = null;

        private readonly CompositeDisposable _disposable = new CompositeDisposable();

        void IInitializable.Initialize()
        {
            Observable.EveryUpdate()
                .Where(_ => Input.GetKeyDown(KeyCode.Space))
                .Select(_ => 1)
                .Scan((acc, cur) => acc + cur)
                .Subscribe(x => _controller.Open(new RequestToast(ToastType.Sample, x.ToString())))
                .AddTo(_disposable);
        }

        void IDisposable.Dispose()
        {
            _disposable.Dispose();
        }
    }
}
