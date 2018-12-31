using SocialGame.Internal.Loading.Builtin;
using UnityEngine;
using Zenject;

namespace SocialGame.Internal.Loading
{
    internal sealed class LoadingInstaller : MonoInstaller<LoadingInstaller>
    {
        [SerializeField] private SampleLoading _sample = null;

        [SerializeField] private SystemLoading _system = null;

        public override void InstallBindings()
        {
            Container.BindInterfacesTo<LoadingModel>().AsSingle();
            Container.BindInterfacesTo<LoadingFactory>().AsSingle();
            Container.BindInstance(_sample).AsSingle();
            Container.BindInstance(_system).AsSingle();
        }
    }
}