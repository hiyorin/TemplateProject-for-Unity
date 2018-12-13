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
            Container.BindInterfacesTo<TapEffectModel>().AsSingle();
            Container.BindInterfacesTo<TapEffectFactory>().AsSingle();
            Container.BindInstance(_sample).AsSingle();
        }
    }
}
