using System;
using UniRx;

namespace SocialGame.Transition
{
    public interface ITransitionIntent
    {
        IObservable<TransMode> OnTransInAsObservable();

        IObservable<Unit> OnTransOutAsObservable();
    }
}
