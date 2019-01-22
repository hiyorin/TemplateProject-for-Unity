using Zenject;

namespace SocialGame.Internal.Network.Http
{
    internal sealed class HttpInstaller : Installer
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesTo<HttpConnection>().AsSingle();
        }
    }
}