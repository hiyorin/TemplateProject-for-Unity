using System;
using SocialGame.Internal.TapEffect;
using UniRx;

namespace SocialGame.TapEffect
{
    public sealed class TapEffectController : ITapEffectIntent
    {
        private readonly Subject<TapEffectType> _onStart = new Subject<TapEffectType>();

        private readonly Subject<Unit> _onStop = new Subject<Unit>();

        public void Start(TapEffectType type)
        {
            _onStart.OnNext(type);
        }

        public void Stop()
        {
            _onStop.OnNext(Unit.Default);
        }

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
