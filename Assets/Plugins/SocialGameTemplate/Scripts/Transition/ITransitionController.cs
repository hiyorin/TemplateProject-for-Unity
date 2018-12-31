using System;
using UniRx;

namespace SocialGame.Transition
{
    public interface ITransitionController
    {
        IObservable<Unit> TransIn(TransMode tras);
        
        IObservable<Unit> TransOut();
    }
}