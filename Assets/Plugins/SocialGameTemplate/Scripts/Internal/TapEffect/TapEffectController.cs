using System;
using SocialGame.TapEffect;
using UniRx;

namespace SocialGame.Internal.TapEffect
{
    internal sealed class TapEffectController : ITapEffectController, ITapEffectIntent
    {
        private readonly Subject<TapEffectType> _onStart = new Subject<TapEffectType>();

        private readonly Subject<Unit> _onStop = new Subject<Unit>();

        #region ITapEffectController implementation
        void ITapEffectController.Start(TapEffectType type)
        {
            _onStart.OnNext(type);
        }

        void ITapEffectController.Stop()
        {
            _onStop.OnNext(Unit.Default);
        }
        #endregion

        #region ITapEffectIntent implementation
        IObservable<TapEffectType> ITapEffectIntent.OnStartAsObservable()
        {
            return _onStart;
        }

        IObservable<Unit> ITapEffectIntent.OnStopAsObservable()
        {
            return _onStop;
        }
        #endregion
    }
}
