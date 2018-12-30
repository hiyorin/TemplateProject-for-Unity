using UnityEngine;
using Zenject;

namespace Sandbox.Loading
{
    internal sealed class LoadingExampleInstaller : MonoInstaller<LoadingExampleInstaller>
    {
        [SerializeField] private LoadingExampleIntent _intent = null;
        
        public override void InstallBindings()
        {
            Container.BindInterfacesTo<LoadingExampleModel>().AsSingle();
            Container.BindInstance<ILoadingExampleIntent>(_intent).AsSingle();
        }
    }
}