using System;
using SocialGame.Transition;
using UniRx;

namespace SocialGame.Internal.Transition
{
    internal sealed class TransitionController : ITransitionController, ITransitionIntent
    {
        private readonly Subject<string> _onTransIn = new Subject<string>();

        private readonly Subject<Unit> _onTransOut = new Subject<Unit>();

        #region ITransitionIntent implementation
        IObservable<Unit> ITransitionController.TransIn(TransMode trans)
        {
            _onTransIn.OnNext(trans.ToString());
            return Observable.Timer(TimeSpan.FromSeconds(0.5f)).AsUnitObservable();
        }

        IObservable<Unit> ITransitionController.TransIn(string name)
        {
            _onTransIn.OnNext(name);
            return Observable.Timer(TimeSpan.FromSeconds(0.5f)).AsUnitObservable();
        }

        IObservable<Unit> ITransitionController.TransOut()
        {
            _onTransOut.OnNext(Unit.Default);
            return Observable.Timer(TimeSpan.FromSeconds(0.5f)).AsUnitObservable();
        }
        #endregion

        #region ITransitionIntent implementation
        IObservable<string> ITransitionIntent.OnTransInAsObservable()
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
