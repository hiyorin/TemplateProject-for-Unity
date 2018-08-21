using System;
using SocialGame.Internal.Sound;
using UniRx;

namespace SocialGame.Sound
{
    public sealed class SoundController : IBGMIntent, ISEIntent, IVoiceIntent
    {
        private readonly BoolReactiveProperty _initialized = new BoolReactiveProperty();

        private readonly Subject<BGM> _onPlayBGM = new Subject<BGM>();

        private readonly Subject<Unit> _onStopBGM = new Subject<Unit>();

        private readonly Subject<SE> _onPlaySE = new Subject<SE>();

        private readonly Subject<Voice> _onPlayVoice = new Subject<Voice>();

        private readonly Subject<Unit> _onStopVoice = new Subject<Unit>();

        public void PlayBGM(BGM value)
        {
            _onPlayBGM.OnNext(value);
        }

        public void StopBGM()
        {
            _onStopBGM.OnNext(Unit.Default);
        }

        public void PlaySE(SE value)
        {
            _onPlaySE.OnNext(value);
        }

        public void PlayVoice(Voice value)
        {
            _onPlayVoice.OnNext(value);
        }

        public void StopVoice()
        {
            _onStopVoice.OnNext(Unit.Default);
        }

        public IObservable<Unit> OnInitializedAsObservable()
        {
            return _initialized
                .Where(x => x)
                .AsUnitObservable();
        }

        #region IBGMIntent implementation
        IObservable<BGM> IBGMIntent.OnPlayAsObservable()
        {
            return _onPlayBGM;
        }

        IObservable<Unit> IBGMIntent.OnStopAsObservable()
        {
            return _onStopBGM;
        }
        #endregion

        #region ISEIntent implementation
        IObservable<SE> ISEIntent.OnPlayAsObservable()
        {
            return _onPlaySE;
        }
        #endregion

        #region IVoiceModel implementation
        IObservable<Voice> IVoiceIntent.OnPlayAsObservable()
        {
            return _onPlayVoice;
        }

        IObservable<Unit> IVoiceIntent.OnStopAsObservable()
        {
            return _onStopVoice;
        }
        #endregion
    }
}
