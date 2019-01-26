using System;
using SocialGame.Transition;
using UniRx;

namespace SocialGame.Internal.Transition
{
    internal interface ITransitionIntent
    {
        IObservable<string> OnTransInAsObservable();

        IObservable<Unit> OnTransOutAsObservable();
    }
}
