using System;
using Zenject;

namespace Sandbox.DebugMode
{
    public sealed class DebugTestInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<DebugTestModel>().AsSingle();
        }
    }
}
