using SocialGame.Internal.Network.gRPC;
using SocialGame.Internal.Network.HTTP;
using UnityEngine;
using Zenject;

namespace SocialGame.Internal.Network
{
    internal sealed class NetworkInstaller : Installer
    {
        public override void InstallBindings()
        {
            var networkSettings = Resources.Load<NetworkSettings>(NetworkSettings.FileName);
            Container.BindInstance(networkSettings.General);

            switch (networkSettings.Server)
            {
                case Server.REST:
                    Container.BindInstance(networkSettings.Http).AsSingle();
                    Container.Install<HttpInstaller>();
                    break;
                case Server.gRPC:
#if SGT_GRPC
                    Container.BindInstance(networkSettings.gRPC).AsSingle();
                    Container.Install<gRPCInstaller>();
#endif
                    break;
                default:
                    Debug.unityLogger.LogError(GetType().Name, $"{networkSettings.Server} is not implemented.");
                    break;
            }
        }
    }
}
