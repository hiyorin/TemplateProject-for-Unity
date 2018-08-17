using Zenject;

namespace Sandbox.Toast
{
    public class ToastTestInstaller : MonoInstaller<ToastTestInstaller>
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<ToastTestModel>().AsSingle();
        }
    }
}
