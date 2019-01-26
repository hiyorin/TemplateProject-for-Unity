using System;
using UniRx;

namespace SocialGame.Transition
{
    public interface ITransitionController
    {
        IObservable<Unit> TransIn(TransMode trans);

        IObservable<Unit> TransIn(string name);

        IObservable<Unit> TransOut();
    }
}