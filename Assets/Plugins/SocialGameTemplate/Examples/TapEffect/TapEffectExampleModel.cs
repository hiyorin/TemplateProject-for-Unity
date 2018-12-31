using System;
using SocialGame.TapEffect;
using Zenject;
using UniRx;

namespace Sandbox.TapEffect
{
    internal interface ITapEffectExampleModel
    {
        IObservable<string> OnChangeMessageAsObservable();
    }
    
    internal sealed class TapEffectExampleModel : IInitializable, IDisposable, ITapEffectExampleModel
    {
        [Inject] private ITapEffectExampleIntent _intent = null;
        
        [Inject] private ITapEffectController _controller = null;

        private readonly StringReactiveProperty _message = new StringReactiveProperty();
        
        private readonly CompositeDisposable _disposable = new CompositeDisposable();

        void IInitializable.Initialize()
        {
            _intent.OnClickShowButtonAsObservable()
                .Do(_ => _controller.Start(TapEffectType.Sample))
                .Subscribe(_ => _message.Value = "Show")
                .AddTo(_disposable);

            _intent.OnClickHideButtonAsObservable()
                .Do(_ => _controller.Stop())
                .Subscribe(_ => _message.Value = "Hide")
                .AddTo(_disposable);
        }

        void IDisposable.Dispose()
        {
            _disposable.Dispose();
        }

        IObservable<string> ITapEffectExampleModel.OnChangeMessageAsObservable()
        {
            return _message;
        }
    }
}