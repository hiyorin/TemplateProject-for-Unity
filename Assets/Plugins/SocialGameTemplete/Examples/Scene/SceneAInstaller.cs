using SocialGame.Scene;
using Zenject;

namespace Sandbox.Scene
{
    public class SceneAInstaller : SceneInstaller
    {
        protected override void OnInstallBindings()
        {
            Container.BindInterfacesAndSelfTo<SceneAModel>().AsSingle();
        }
    }
}
