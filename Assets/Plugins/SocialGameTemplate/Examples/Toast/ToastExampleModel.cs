using System;
using UnityEngine;
using SocialGame.Toast;
using Zenject;
using UniRx;

namespace Sandbox.Toast
{
    internal interface IToastExampleModel
    {
        IObservable<string> OnChangeMessageAsObservable();
    }
    
    internal sealed class ToastExampleModel : IInitializable, IDisposable, IToastExampleModel
    {
        [Inject] private IToastExampleIntent _intent = null;
        
        [Inject] private IToastController _controller = null;

        private readonly StringReactiveProperty _message = new StringReactiveProperty();
        
        private readonly CompositeDisposable _disposable = new CompositeDisposable();

        void IInitializable.Initialize()
        {
            _intent.OnClickShowButtonAsObservable()
                .Select(_ => 1)
                .Scan((acc, cur) => acc + cur)
                .Do(x => _controller.Open(new RequestToast(ToastType.Sample, x.ToString())))
                .Subscribe(x => _message.Value = $"Show {x}")
                .AddTo(_disposable);
        }

        void IDisposable.Dispose()
        {
            _disposable.Dispose();
        }

        IObservable<string> IToastExampleModel.OnChangeMessageAsObservable()
        {
            return _message;
        }
    }
}
