using System;
using SocialGame.Transition;
using UniRx;

namespace SocialGame.Internal.Transition
{
    public interface ITransitionIntent
    {
        IObservable<TransMode> OnTransInAsObservable();

        IObservable<Unit> OnTransOutAsObservable();
    }
}
