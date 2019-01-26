using System;
using SocialGame.TapEffect;
using UniRx;

namespace SocialGame.Internal.TapEffect
{
    internal sealed class TapEffectController : ITapEffectController, ITapEffectIntent
    {
        private readonly Subject<string> _onStart = new Subject<string>();

        private readonly Subject<Unit> _onStop = new Subject<Unit>();

        #region ITapEffectController implementation
        void ITapEffectController.Start(TapEffectType type)
        {
            _onStart.OnNext(type.ToString());
        }

        void ITapEffectController.Start(string name)
        {
            _onStart.OnNext(name);
        }
        
        void ITapEffectController.Stop()
        {
            _onStop.OnNext(Unit.Default);
        }
        #endregion

        #region ITapEffectIntent implementation
        IObservable<string> ITapEffectIntent.OnStartAsObservable()
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
