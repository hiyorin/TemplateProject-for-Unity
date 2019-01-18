using System;
using UnityEngine;
using SocialGame.Data;
using Zenject;
using UniRx;

namespace Sandbox.Resolution
{
    internal interface IResolutionExampleModel
    {
        IObservable<string> OnChangeMessageAsObservable();
    }
    
    internal sealed class ResolutionExampleModel : IInitializable, IDisposable, IResolutionExampleModel
    {
        [Inject] private IResolutionExampleIntent _intent = null;
        
        [Inject] private IResolutionController _controller = null;

        private readonly StringReactiveProperty _message = new StringReactiveProperty();
        
        private readonly CompositeDisposable _disposable = new CompositeDisposable();
        
        void IInitializable.Initialize()
        {
            _controller.Get()
                .ToObservable()
                .Subscribe(x => _message.Value = $"{x} {Screen.width}x{Screen.height}")
                .AddTo(_disposable);
            
            Observable.Merge(
                    _intent.OnClickLowButtonAsObservable().Select(_ => Quality.Low),
                    _intent.OnClickMiddleButtonAsObservable().Select(_ => Quality.Middle),
                    _intent.OnClickHighButtonAsObservable().Select(_ => Quality.High))
                .SelectMany(x => _controller.Put(x).ToObservable())
                .Subscribe(x => _message.Value = $"{x} {Screen.width}x{Screen.height}")
                .AddTo(_disposable);
        }

        void IDisposable.Dispose()
        {
            _disposable.Dispose();
        }

        IObservable<string> IResolutionExampleModel.OnChangeMessageAsObservable()
        {
            return _message;
        }
    }
}
