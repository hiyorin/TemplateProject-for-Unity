using System;
using SocialGame.TapEffect;
using UnityEngine;
using Zenject;

namespace SocialGame.Internal.TapEffect
{
    internal sealed class TapEffectInstaller : MonoInstaller<TapEffectInstaller>
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
