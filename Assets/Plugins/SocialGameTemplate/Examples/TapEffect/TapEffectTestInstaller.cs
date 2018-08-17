using UnityEngine;
using Zenject;

namespace Sandbox.TapEffect
{
    public class TapEffectTestInstaller : MonoInstaller<TapEffectTestInstaller>
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<TapEffectTestModel>().AsSingle();
        }
    }
}
