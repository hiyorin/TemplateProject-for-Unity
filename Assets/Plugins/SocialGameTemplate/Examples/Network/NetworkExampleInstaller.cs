using Sandbox.Network.Data;
using UnityEngine;
using Zenject;

namespace Sandbox.Network
{
    internal sealed class NetworkExampleInstaller : MonoInstaller
    {
        [SerializeField] private NetworkExampleIntent _intent = null;
        
        public override void InstallBindings()
        {
            Container.BindInterfacesTo<NetworkExampleModel>().AsSingle();
            Container.BindInstance<INetworkExampleIntent>(_intent).AsSingle();

            #if SGT_GRPC
            Container.BindInterfacesTo<HelloworldDataStore>().AsSingle();
            #endif
        }
    }
}
