using UnityEngine;
using Zenject;

namespace SocialGame.Transition
{
    public class TransitionFactory
    {
        [Inject] private DiContainer _container = null;

        [Inject] private TransitionSettings _settings = null;

        [Inject] private BlackFadeTransition _blackFade = null;

        public GameObject Create(TransMode trans)
        {
            if (trans == TransMode.BlackFade)
                return _container.InstantiatePrefab(_blackFade);
            else
                return _container.InstantiatePrefab(_settings.Prefabs [(int)trans]);
        }
    }
}
