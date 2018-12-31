using SocialGame.Internal.Toast.Builtin;
using UnityEngine;
using Zenject;

namespace SocialGame.Internal.Toast
{
    internal sealed class ToastInstaller : MonoInstaller<ToastInstaller>
    {
        [SerializeField] private SampleToast _sample = null;

        public override void InstallBindings()
        {
            Container.BindInterfacesTo<ToastModel>().AsSingle();
            Container.BindInterfacesTo<ToastFactory>().AsSingle();
            Container.BindInstance(_sample).AsSingle();
        }
    }
}
