using System;
using SocialGame.Transition;
using UniRx;

namespace SocialGame.Internal.Transition
{
    internal sealed class TransitionController : ITransitionController, ITransitionIntent
    {
        private readonly Subject<TransMode> _onTransIn = new Subject<TransMode>();

        private readonly Subject<Unit> _onTransOut = new Subject<Unit>();

        #region ITransitionIntent implementation
        IObservable<Unit> ITransitionController.TransIn(TransMode tras)
        {
            _onTransIn.OnNext(tras);
            return Observable.Timer(TimeSpan.FromSeconds(0.5f)).AsUnitObservable();
        }

        IObservable<Unit> ITransitionController.TransOut()
        {
            _onTransOut.OnNext(Unit.Default);
            return Observable.Timer(TimeSpan.FromSeconds(0.5f)).AsUnitObservable();
        }
        #endregion

        #region ITransitionIntent implementation
        IObservable<TransMode> ITransitionIntent.OnTransInAsObservable()
        {
            return _onTransIn;
        }

        IObservable<Unit> ITransitionIntent.OnTransOutAsObservable()
        {
            return _onTransOut;
        }
        #endregion
        
    }
}
