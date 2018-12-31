using System;
using UniRx;

namespace SocialGame.Transition
{
    public interface ITransition
    {
        IObservable<Unit> OnTransInAsObservable(float defaultDuration);

        IObservable<Unit> OnTransOutAsObservable(float defaultDuration);
    }
}
