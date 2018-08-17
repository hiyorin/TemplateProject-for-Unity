using System;
using SocialGame.Internal.Transition;
using UniRx;

namespace SocialGame.Transition
{
    public sealed class TransitionController : ITransitionIntent
    {
        private readonly Subject<TransMode> _onTransIn = new Subject<TransMode>();

        private readonly Subject<Unit> _onTransOut = new Subject<Unit>();

        public IObservable<Unit> TransIn(TransMode tras)
        {
            _onTransIn.OnNext(tras);
            return Observable.Timer(TimeSpan.FromSeconds(0.5f)).AsUnitObservable();
        }

        public IObservable<Unit> TransOut()
        {
            _onTransOut.OnNext(Unit.Default);
            return Observable.Timer(TimeSpan.FromSeconds(0.5f)).AsUnitObservable();
        }

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
