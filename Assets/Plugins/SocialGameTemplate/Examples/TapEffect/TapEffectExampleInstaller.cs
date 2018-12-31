using UnityEngine;
using Zenject;

namespace Sandbox.TapEffect
{
    internal sealed class TapEffectExampleInstaller : MonoInstaller<TapEffectExampleInstaller>
    {
        [SerializeField] private TapEffectExampleIntent _intent = null;
        
        public override void InstallBindings()
        {
            Container.BindInterfacesTo<TapEffectExampleModel>().AsSingle();
            Container.BindInstance<ITapEffectExampleIntent>(_intent).AsSingle();
        }
    }
}
