using System;
using System.Linq;
using SocialGame.Sound;
using UniRx;
using UnityEngine;
using Zenject;

namespace SocialGame.Internal.Sound
{
    internal interface IBGMIntent
    {
        IObservable<BGM> OnPlayAsObservable();

        IObservable<string> OnPlayForNameAsObservable();

        IObservable<bool> OnPauseAsObservable();

        IObservable<Unit> OnStopAsObservable();
    }

    internal interface ISEIntent
    {
        IObservable<SE> OnPlayAsObservable();

        IObservable<string> OnPlayForNameAsObservable();
    }
    
    internal interface IVoiceIntent
    {
        IObservable<Voice> OnPlayAsObservable();

        IObservable<string> OnPlayForNameAsObservable();

        IObservable<Unit> OnStopAsObservable();
    }

    internal sealed class SoundController : IInitializable, IDisposable, ISoundController, IBGMIntent, ISEIntent, IVoiceIntent
    {
        [Inject] private ISoundModel[] _models = null;

        private readonly BoolReactiveProperty _initialized = new BoolReactiveProperty();
        
        private readonly Subject<BGM> _onPlayBGM = new Subject<BGM>();
        
        private readonly Subject<string> _onPlayBGMForName = new Subject<string>();

        private readonly Subject<bool> _onPauseBGM = new Subject<bool>();
        
        private readonly Subject<Unit> _onStopBGM = new Subject<Unit>();

        private readonly Subject<SE> _onPlaySE = new Subject<SE>();
        
        private readonly Subject<string> _onPlaySEForName = new Subject<string>();

        private readonly Subject<Voice> _onPlayVoice = new Subject<Voice>();

        private readonly Subject<string> _onPlayVoiceForName = new Subject<string>();
        
        private readonly Subject<Unit> _onStopVoice = new Subject<Unit>();

        private readonly CompositeDisposable _disposable = new CompositeDisposable();
        
        void IInitializable.Initialize()
        {
            _models
                .Select(x => x.OnInitializedAsObservable().First())
                .WhenAll()
                .First()
                .Subscribe(_ => _initialized.Value = true)
                .AddTo(_disposable);
        }

        void IDisposable.Dispose()
        {
            _disposable.Dispose();
        }
        
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

        void ISoundController.PlayBGM(string name)
        {
            _onPlayBGMForName.OnNext(name);
        }

        void ISoundController.PauseBGM(bool pause)
        {
            _onPauseBGM.OnNext(pause);
        }
        
        void ISoundController.StopBGM()
        {
            _onStopBGM.OnNext(Unit.Default);
        }

        void ISoundController.PlaySE(SE value)
        {
            _onPlaySE.OnNext(value);
        }

        void ISoundController.PlaySE(string name)
        {
            _onPlaySEForName.OnNext(name);
        }
        
        void ISoundController.PlayVoice(Voice value)
        {
            _onPlayVoice.OnNext(value);
        }

        void ISoundController.PlayVoice(string name)
        {
            _onPlayVoiceForName.OnNext(name);
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

        IObservable<string> IBGMIntent.OnPlayForNameAsObservable()
        {
            return _onPlayBGMForName;
        }

        IObservable<bool> IBGMIntent.OnPauseAsObservable()
        {
            return _onPauseBGM;
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

        IObservable<string> ISEIntent.OnPlayForNameAsObservable()
        {
            return _onPlaySEForName;
        }
        #endregion

        #region IVoiceModel implementation
        IObservable<Voice> IVoiceIntent.OnPlayAsObservable()
        {
            return _onPlayVoice;
        }

        IObservable<string> IVoiceIntent.OnPlayForNameAsObservable()
        {
            return _onPlayVoiceForName;
        }
        
        IObservable<Unit> IVoiceIntent.OnStopAsObservable()
        {
            return _onStopVoice;
        }
        #endregion
    }
}
