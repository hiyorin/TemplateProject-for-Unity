using System;
using UnityEngine;
using UniRx;

namespace SocialGame.Transition
{
    public interface ITransitionModel
    {
        IObservable<GameObject> OnAddAsObservable();

        IObservable<Unit> OnTransInCompleteAsObservalbe();

        IObservable<Unit> OnTransOutCompleteAsObservable();
    }
}
