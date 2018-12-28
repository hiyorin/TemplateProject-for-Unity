using System;
using SocialGame.Sound;
using UniRx;

namespace SocialGame.Internal.Sound
{
    internal interface IBGMIntent
    {
        IObservable<BGM> OnPlayAsObservable();

        IObservable<Unit> OnStopAsObservable();
    }

    internal interface ISEIntent
    {
        IObservable<SE> OnPlayAsObservable();
    }
    
    internal interface IVoiceIntent
    {
        IObservable<Voice> OnPlayAsObservable();
        IObservable<Unit> OnStopAsObservable();
    }

    internal sealed class SoundController : ISoundController, IBGMIntent, ISEIntent, IVoiceIntent
    {
        private readonly BoolReactiveProperty _initialized = new BoolReactiveProperty();

        private readonly Subject<BGM> _onPlayBGM = new Subject<BGM>();

        private readonly Subject<Unit> _onStopBGM = new Subject<Unit>();

        private readonly Subject<SE> _onPlaySE = new Subject<SE>();

        private readonly Subject<Voice> _onPlayVoice = new Subject<Voice>();

        private readonly Subject<Unit> _onStopVoice = new Subject<Unit>();

        #region ISoundController implementation
        IObservable<Unit> ISoundController.OnInitializedAsObservable()
        {
            return _initialized
                .Where(x => x)
                .AsUnitObservable();
        }
        
        void ISoundController.PlayBGM(BGM value)
        {
            _onPlayBGM.OnNext(value);
        }

        void ISoundController.StopBGM()
        {
            _onStopBGM.OnNext(Unit.Default);
        }

        void ISoundController.PlaySE(SE value)
        {
            _onPlaySE.OnNext(value);
        }

        void ISoundController.PlayVoice(Voice value)
        {
            _onPlayVoice.OnNext(value);
        }
        
        void ISoundController.StopVoice()
        {
            _onStopVoice.OnNext(Unit.Default);
        }
        #endregion

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
