using SocialGame.TapEffect;
using UnityEngine;
using Zenject;

namespace SocialGame.Internal.TapEffect
{
    internal sealed class TapEffectFactory
    {
        [Inject] private DiContainer _container = null;

        [Inject] private TapEffectSettings _settings = null;

        [Inject] private SampleTapEffect _samplePrefab = null;

        public GameObject Create(TapEffectType type)
        {
            if (type == TapEffectType.Sample)
                return _container.InstantiatePrefab(_samplePrefab);
            else
                return _container.InstantiatePrefab(_settings.Prefabs[(int)type]);
        }
    }
}
