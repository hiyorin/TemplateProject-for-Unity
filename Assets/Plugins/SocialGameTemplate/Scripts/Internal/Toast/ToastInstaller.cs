using System;
using SocialGame.Toast;
using UnityEngine;
using Zenject;

namespace SocialGame.Internal.Toast
{
    internal sealed class ToastInstaller : MonoInstaller<ToastInstaller>
    {
        [SerializeField] private SampleToast _sample = null;

        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<ToastModel>().AsSingle();
            Container.BindInterfacesAndSelfTo<ToastFactory>().AsSingle();
            Container.BindInstance(_sample).AsSingle();
        }
    }
}
