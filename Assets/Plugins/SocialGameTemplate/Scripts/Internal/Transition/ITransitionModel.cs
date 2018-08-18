using System;
using UnityEngine;
using UniRx;

namespace SocialGame.Internal.Transition
{
    internal interface ITransitionModel
    {
        IObservable<GameObject> OnAddAsObservable();

        IObservable<Unit> OnTransInCompleteAsObservable();

        IObservable<Unit> OnTransOutCompleteAsObservable();
    }
}
