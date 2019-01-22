#if SGT_GRPC
using Zenject;

namespace SocialGame.Internal.Network.Grpc
{
    internal sealed class GrpcInstaller : Installer
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesTo<GrpcConnection>().AsSingle();
        }
    }
}
#endif