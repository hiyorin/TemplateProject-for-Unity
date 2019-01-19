using Zenject;

namespace SocialGame.Internal.Network.HTTP
{
    internal sealed class HttpInstaller : Installer
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesTo<HttpConnection>().AsSingle();
        }
    }
}