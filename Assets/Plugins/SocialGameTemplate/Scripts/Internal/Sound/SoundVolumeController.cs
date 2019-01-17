using System;
using SocialGame.Data.Entity;
using SocialGame.Internal.Data.DataStore;
using SocialGame.Sound;
using Zenject;
using UniRx;

namespace SocialGame.Internal.Sound
{
    internal sealed class SoundVolumeController : IInitializable, IDisposable, ISoundVolumeController, ISoundVolumeIntent
    {
        [Inject] private SoundVolumeLocalStorage _localStorage = null;

        private readonly FloatReactiveProperty _master = new FloatReactiveProperty();

        private readonly FloatReactiveProperty _bgm = new FloatReactiveProperty();
        
        private readonly FloatReactiveProperty _se = new FloatReactiveProperty();
        
        private readonly FloatReactiveProperty _voice = new FloatReactiveProperty();
        
        private readonly CompositeDisposable _disposable = new CompositeDisposable();

        void IInitializable.Initialize()
        {
            var settings = _localStorage.Model; 
            _master.Value = settings.Master;
            _bgm.Value = settings.BGM;
            _se.Value = settings.SE;
            _voice.Value = settings.Voice;

            _master
                .Subscribe(x => _localStorage.Model.Master = x)
                .AddTo(_disposable);

            _bgm
                .Subscribe(x => _localStorage.Model.BGM = x)
                .AddTo(_disposable);

            _se
                .Subscribe(x => _localStorage.Model.SE = x)
                .AddTo(_disposable);

            _voice
                .Subscribe(x => _localStorage.Model.Voice = x)
                .AddTo(_disposable);
        }

        void IDisposable.Dispose()
        {
            _disposable.Dispose();
        }

        #region ISoundVolumeController implementation
        SoundVolume ISoundVolumeController.Get()
        {
            return _localStorage.Model.Clone() as SoundVolume;
        }

        void ISoundVolumeController.Put(SoundVolume value)
        {
            if (value == null)
                throw new ArgumentException();
            
            _master.Value = value.Master;
            _bgm.Value = value.BGM;
            _se.Value = value.SE;
            _voice.Value = value.Voice;
        }

        IObservable<Unit> ISoundVolumeController.Save()
        {
            return _localStorage.SaveAsync();
        }
        #endregion
        
        #region ISoundVolumeIntent implementation
        IObservable<float> ISoundVolumeIntent.OnMasterVolumeAsObservable()
        {
            return _master;
        }

        IObservable<float> ISoundVolumeIntent.OnBGMVolumeAsObservable()
        {
            return _bgm;
        }

        IObservable<float> ISoundVolumeIntent.OnSEVolumeAsObservable()
        {
            return _se;
        }

        IObservable<float> ISoundVolumeIntent.OnVoiceVolumeAsObservable()
        {
            return _voice;
        }
        #endregion
    }
}
