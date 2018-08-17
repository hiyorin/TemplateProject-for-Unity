using Zenject;

namespace Sandbox.Dialog
{
    public class DialogTestInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<DialogTestModel>().AsSingle();
        }
    }
}
