using System;
using Sandbox.Network.Data;
using UniRx;
using Zenject;

namespace Sandbox.Network
{
    internal interface INetworkExampleModel
    {
        IObservable<string> OnChangeMessageAsObservable();
    }
    
    internal sealed class NetworkExampleModel : IInitializable, IDisposable, INetworkExampleModel
    {
        [Inject] private INetworkExampleIntent _intent = null;

        #if SGT_GRPC
        [Inject] private IHelloworldDataStore
        #else
        [Inject] private IExampleDataStore
        #endif
            _dataStore = null;
        
        private readonly StringReactiveProperty _message = new StringReactiveProperty();
        
        private readonly CompositeDisposable _disposable = new CompositeDisposable();
        
        void IInitializable.Initialize()
        {
            _intent.OnClickRequestButtonAsObservable()
                .SelectMany(_ => _dataStore.SayHello("Taro").ToObservable())
                .Subscribe(x => _message.Value = x)
                .AddTo(_disposable);
        }

        void IDisposable.Dispose()
        {
            _disposable.Dispose();
        }

        IObservable<string> INetworkExampleModel.OnChangeMessageAsObservable()
        {
            return _message;
        }
    }
}