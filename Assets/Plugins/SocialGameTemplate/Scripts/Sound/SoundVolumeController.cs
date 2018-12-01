using System;
using SocialGame.Internal.Sound;
using Zenject;
using UniRx;

namespace SocialGame.Sound
{
    public sealed class SoundVolumeController : IInitializable, IDisposable, ISoundVolumeIntent
    {
        //[Inject] private SoundSettings _generalSettings = null;

        //[Inject] private BGMSettings _bgmSettings = null;

        //[Inject] private SESettings _seSettings = null;

        //[Inject] private VoiceSettings _voiceSettings = null;

        private readonly ReactiveProperty<VolumeSettings> _settings = new ReactiveProperty<VolumeSettings>();

        public VolumeSettings Settings {
            get { return _settings.Value; }
            set { _settings.Value = value; }
        }

        void IInitializable.Initialize()
        {
            //Settings = new VolumeSettings() {
            //    Master = _generalSettings.DefaultVolume,
            //    BGM = _bgmSettings.DefaultVolume,
            //    SE = _seSettings.DefaultVolume,
            //    Voice = _voiceSettings.DefaultVolume,
            //};
        }

        void IDisposable.Dispose()
        {
            
        }

        #region ISoundVolumeIntent implementation
        IObservable<float> ISoundVolumeIntent.OnMasterVolumeAsObservable()
        {
            return _settings
                .Select(x => x.Master);
        }

        IObservable<float> ISoundVolumeIntent.OnBGMVolumeAsObservable()
        {
            return _settings
                .Select(x => x.BGM);
        }

        IObservable<float> ISoundVolumeIntent.OnSEVolumeAsObservable()
        {
            return _settings
                .Select(x => x.SE);
        }

        IObservable<float> ISoundVolumeIntent.OnVoiceVolumeAsObservable()
        {
            return _settings
                .Select(x => x.Voice);
        }
        #endregion
    }
}
