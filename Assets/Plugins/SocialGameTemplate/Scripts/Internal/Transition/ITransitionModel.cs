using System;
using UnityEngine;
using UniRx;

namespace SocialGame.Internal.Transition
{
    internal interface ITransitionModel
    {
        IObservable<GameObject> OnAddAsObservable();

        IObservable<Unit> OnTransInCompleteAsObservalbe();

        IObservable<Unit> OnTransOutCompleteAsObservable();
    }
}
