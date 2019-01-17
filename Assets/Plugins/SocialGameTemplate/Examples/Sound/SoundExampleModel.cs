using System;
using SocialGame.Data.Entity;
using SocialGame.Sound;
using Zenject;
using UniRx;

namespace SocialGame.Examples.Sound
{
    internal interface ISoundExampleModel
    {
        IObservable<string[]> OnAddBgmAsObservable();
        
        IObservable<string[]> OnAddSeAsObservable();
        
        IObservable<string[]> OnAddVoiceAsObservable();

        IObservable<float> OnChangeBgmVolumeAsObservable();
        
        IObservable<float> OnChangeSeVolumeAsObservable();
        
        IObservable<float> OnChangeVoiceVolumeAsObservable();
    }
    
    internal sealed class SoundExampleModel : IInitializable, IDisposable, ISoundExampleModel
    {
        [Inject(Id = SoundType.BGM)] private ISoundExampleIntent _bgmIntent = null;

        [Inject(Id = SoundType.SE)] private ISoundExampleIntent _seIntent = null;

        [Inject(Id = SoundType.Voice)] private ISoundExampleIntent _voiceIntent = null;

        [Inject] private ISoundController _controller = null;

        [Inject] private ISoundVolumeController _volumeController = null;
        
        [Inject] private IProject _project = null;
        
        private readonly FloatReactiveProperty _bgmVolume = new FloatReactiveProperty();
        
        private readonly FloatReactiveProperty _seVolume = new FloatReactiveProperty();
        
        private readonly FloatReactiveProperty _voiceVolume = new FloatReactiveProperty();
        
        private readonly CompositeDisposable _disposable = new CompositeDisposable();
        
        void IInitializable.Initialize()
        {
            string bgm = null, se = null, voice = null;

            var initialize = _project.OnInitializedAsObservable()
                .First()
                .Publish()
                .RefCount();
            
            // select sound
            initialize
                .SelectMany(_ => _bgmIntent.OnSelectAsObservable())
                .Subscribe(x => bgm = x)
                .AddTo(_disposable);

            initialize
                .SelectMany(_ => _seIntent.OnSelectAsObservable())
                .Subscribe(x => se = x)
                .AddTo(_disposable);

            initialize
                .SelectMany(_ => _voiceIntent.OnSelectAsObservable())
                .Subscribe(x => voice = x)
                .AddTo(_disposable);

            // play sound
            _bgmIntent.OnClickPlayButtonAsObservable()
                .Subscribe(_ => _controller.PlayBGM(bgm))
                .AddTo(_disposable);

            _seIntent.OnClickPlayButtonAsObservable()
                .Subscribe(_ => _controller.PlaySE(se))
                .AddTo(_disposable);

            _voiceIntent.OnClickPlayButtonAsObservable()
                .Subscribe(_ => _controller.PlayVoice(voice))
                .AddTo(_disposable);
            
            // pause sound
            _bgmIntent.OnClickPauseButtonAsObservable()
                .Select(_ => true)
                .Scan((cur, acc) => cur ^= acc)
                .Subscribe(x => _controller.PauseBGM(x))
                .AddTo(_disposable);
            
            _seIntent.OnClickPauseButtonAsObservable()
                .Subscribe()
                .AddTo(_disposable);
            
            _voiceIntent.OnClickPauseButtonAsObservable()
                .Subscribe()
                .AddTo(_disposable);
            
            // stop sound
            _bgmIntent.OnClickStopButtonAsObservable()
                .Subscribe(_ => _controller.StopBGM())
                .AddTo(_disposable);
            
            _seIntent.OnClickStopButtonAsObservable()
                .Subscribe()
                .AddTo(_disposable);
            
            _voiceIntent.OnClickStopButtonAsObservable()
                .Subscribe(_ => _controller.StopVoice())
                .AddTo(_disposable);
            
            // volume
            var volume = _volumeController.Get();
            _bgmVolume.Value = volume.BGM;
            _seVolume.Value = volume.SE;
            _voiceVolume.Value = volume.Voice;
            
            Observable.Merge(
                    _bgmIntent.OnVolumeAsObservable().Skip(1).Do(x => _bgmVolume.Value = x),
                    _seIntent.OnVolumeAsObservable().Skip(1).Do(x => _seVolume.Value = x),
                    _voiceIntent.OnVolumeAsObservable().Skip(1).Do(x => _voiceVolume.Value = x))
                .Subscribe(x => _volumeController.Put(new SoundVolume()
                {
                    Master = 1.0f,
                    BGM = _bgmVolume.Value,
                    SE = _seVolume.Value,
                    Voice = _voiceVolume.Value,
                }))
                .AddTo(_disposable);
        }

        void IDisposable.Dispose()
        {
            _disposable.Dispose();
        }

        IObservable<string[]> ISoundExampleModel.OnAddBgmAsObservable()
        {
            return Observable.Return(Enum.GetNames(typeof(BGM)));
        }
        
        IObservable<string[]> ISoundExampleModel.OnAddSeAsObservable()
        {
            return Observable.Return(Enum.GetNames(typeof(SE)));
        }
        
        IObservable<string[]> ISoundExampleModel.OnAddVoiceAsObservable()
        {
            return Observable.Return(Enum.GetNames(typeof(Voice)));
        }

        IObservable<float> ISoundExampleModel.OnChangeBgmVolumeAsObservable()
        {
            return _bgmVolume;
        }

        IObservable<float> ISoundExampleModel.OnChangeSeVolumeAsObservable()
        {
            return _seVolume;
        }

        IObservable<float> ISoundExampleModel.OnChangeVoiceVolumeAsObservable()
        {
            return _voiceVolume;
        }
    }
}