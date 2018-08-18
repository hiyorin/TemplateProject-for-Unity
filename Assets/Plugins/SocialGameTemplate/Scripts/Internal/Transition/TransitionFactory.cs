using SocialGame.Transition;
using UnityEngine;
using Zenject;

namespace SocialGame.Internal.Transition
{
    internal sealed class TransitionFactory : ITransitionFactory
    {
        [Inject] private DiContainer _container = null;

        [Inject] private TransitionSettings _settings = null;

        [Inject] private BlackFadeTransition _blackFade = null;

        #region ITransitionFactory implementation
        GameObject ITransitionFactory.Create(TransMode trans)
        {
            if (trans == TransMode.BlackFade)
                return _container.InstantiatePrefab(_blackFade);
            else
                return _container.InstantiatePrefab(_settings.Prefabs[(int)trans]);
        }
        #endregion
    }
}
