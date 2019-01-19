using System;
using UniRx;
using Zenject;

namespace Sandbox.Network
{
    internal interface INetworkExampleModel
    {
        
    }
    
    internal sealed class NetworkExampleModel : IInitializable, IDisposable, INetworkExampleModel
    {
        [Inject] private INetworkExampleIntent _intent = null;

        private readonly CompositeDisposable _disposable = new CompositeDisposable();
        
        void IInitializable.Initialize()
        {
            
        }

        void IDisposable.Dispose()
        {
            _disposable.Dispose();
        }
    }
}