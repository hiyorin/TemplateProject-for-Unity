using Zenject;

namespace Sandbox.Loading
{
    public class LoadingTestInstaller : MonoInstaller<LoadingTestInstaller>
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<LoadingTestModel>().AsSingle();
        }
    }
}