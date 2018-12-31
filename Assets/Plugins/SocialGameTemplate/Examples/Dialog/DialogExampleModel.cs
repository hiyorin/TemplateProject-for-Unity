using System;
using SocialGame.Dialog;
using UnityEngine;
using Zenject;
using UniRx;

namespace Sandbox.Dialog
{
    internal interface IDialogExampleModel
    {
        IObservable<string> OnChangeMessageAsObservable();
    }
    
    internal sealed class DialogExampleModel : IInitializable, IDisposable, IDialogExampleModel
    {
        [Inject] private IDialogExampleIntent _intent = null;
        
        [Inject] private IDialogController _dialogController = null;

        private readonly StringReactiveProperty _message = new StringReactiveProperty();
        
        private readonly CompositeDisposable _disposable = new CompositeDisposable();

        void IInitializable.Initialize()
        {
            _intent.OnClickOpenButtonAsObservable()
                .SelectMany(_ => _dialogController.Open<string>(DialogType.Sample, UnityEngine.Random.Range(0, 100).ToString()))
                .Subscribe(x => _message.Value = "Result " + x)
                .AddTo(_disposable);

            _intent.OnClickOpenPrimaryButtonAsObservable()
                .SelectMany(_ => _dialogController.OpenPrimary<string>(DialogType.Sample, "Primary"))
                .Subscribe(x => _message.Value = "Result Primary " + x)
                .AddTo(_disposable);
        }

        void IDisposable.Dispose()
        {
            _disposable.Dispose();
        }

        IObservable<string> IDialogExampleModel.OnChangeMessageAsObservable()
        {
            return _message;
        }
    }
}
