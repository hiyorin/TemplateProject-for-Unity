using System;
using UnityEngine;
using UnityEngine.UI;
using UnityExtensions;
using Zenject;
using UniRx;
using DG.Tweening;

namespace SocialGame.Transition
{
    public sealed class BlackFadeTransition : MonoBehaviour, ITransition
    {
        [SerializeField] private Graphic _background = null;

        [Inject] private TransitionSettings _settings = null;

        #region ITransition implementation
        IObservable<Unit> ITransition.OnTransInAsObservable()
        {
            return _background
                .DOFade(1.0f, _settings.DefaultDuration)
                .OnCompleteAsObservable();
        }

        IObservable<Unit> ITransition.OnTransOutAsObservable()
        {
            return _background
                .DOFade(0.0f, _settings.DefaultDuration)
                .OnCompleteAsObservable();
        }
        #endregion
    }
}
