using System;
using SocialGame.Transition;
using UnityEngine;
using UnityEngine.UI;
using UnityExtensions;
using UniRx;
using DG.Tweening;

namespace SocialGame.Internal.Transition.Builtin
{
    internal sealed class BlackFadeTransition : MonoBehaviour, ITransition
    {
        [SerializeField] private Graphic _background = null;

        #region ITransition implementation
        IObservable<Unit> ITransition.OnTransInAsObservable(float defaultDuration)
        {
            return _background
                .DOFade(1.0f, defaultDuration)
                .OnCompleteAsObservable();
        }

        IObservable<Unit> ITransition.OnTransOutAsObservable(float defaultDuration)
        {
            return _background
                .DOFade(0.0f, defaultDuration)
                .OnCompleteAsObservable();
        }
        #endregion
    }
}
