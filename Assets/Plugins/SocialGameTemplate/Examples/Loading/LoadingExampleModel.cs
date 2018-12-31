using System;
using SocialGame.Loading;
using Zenject;
using UniRx;

namespace Sandbox.Loading
{
    internal interface ILoadingExampleModel
    {
        IObservable<string> OnChangeMessageAsObservable();
    }
    
    internal sealed class LoadingExampleModel : IInitializable, IDisposable, ILoadingExampleModel
    {
        [Inject] private ILoadingExampleIntent _intent = null;
        
        [Inject] private ILoadingController _controller = null;

        private readonly StringReactiveProperty _message = new StringReactiveProperty();
        
        private readonly CompositeDisposable _disposable = new CompositeDisposable();

        void IInitializable.Initialize()
        {
            Observable.Merge(
                    _intent.OnClickShowSampleButtonAsObservable().Select(_ => LoadingType.Sample),
                    _intent.OnClickShowSystemButtonAsObservable().Select(_ => LoadingType.System))
                .Do(x => _controller.Show(x))
                .Do(_ => _message.Value = "Show")
                .SelectMany(_ => Observable.Timer(TimeSpan.FromSeconds(2.0f)))
                .Do(_ => _controller.Hide())
                .Do(_ => _message.Value = "Hide")
                .Subscribe()
                .AddTo(_disposable);
        }

        void IDisposable.Dispose()
        {
            _disposable.Dispose();
        }

        IObservable<string> ILoadingExampleModel.OnChangeMessageAsObservable()
        {
            return _message;
        }
    }
}