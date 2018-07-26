using System;
using UnityEngine;
using Zenject;

namespace SocialGame.TapEffect
{
    public sealed class TapEffectInstaller : MonoInstaller<TapEffectInstaller>
    {
        [SerializeField] private SampleTapEffect _sample = null;

        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<TapEffectModel>().AsSingle();
            Container.BindInterfacesAndSelfTo<TapEffectFactory>().AsSingle();
            Container.BindInstance(_sample).AsSingle();
        }
    }
}
