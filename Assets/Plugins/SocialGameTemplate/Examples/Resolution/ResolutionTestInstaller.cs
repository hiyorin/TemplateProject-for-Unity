using Zenject;

namespace Sandbox.Resolution
{
    public class ResolutionTestInstaller : MonoInstaller<ResolutionTestInstaller>
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesTo<ResolutionTestModel>().AsSingle();
        }
    }
}
