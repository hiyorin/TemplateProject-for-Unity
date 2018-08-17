using SocialGame.Scene;
using Zenject;

namespace Sandbox.Scene
{
    public class SceneBInstaller : SceneInstaller
    {
        protected override void OnInstallBindings()
        {
            Container.BindInterfacesAndSelfTo<SceneBModel>().AsSingle();
        }
    }
}
