using UnityEngine;
using Zenject;

namespace Sandbox.Resolution
{
    internal sealed class ResolutionExampleInstaller : MonoInstaller<ResolutionExampleInstaller>
    {
        [SerializeField] private ResolutionExampleIntent _intent = null;
        
        public override void InstallBindings()
        {
            Container.BindInterfacesTo<ResolutionExampleModel>().AsSingle();
            Container.BindInstance<IResolutionExampleIntent>(_intent).AsSingle();
        }
    }
}
