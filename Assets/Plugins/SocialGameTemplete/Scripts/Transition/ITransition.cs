using System;
using UniRx;

namespace SocialGame.Transition
{
    public interface ITransition
    {
        IObservable<Unit> OnTransInAsObservable();

        IObservable<Unit> OnTransOutAsObservable();
    }
}
