using System;
using System.Linq;
using UnityEngine;
using UnityExtensions;
using Zenject;
using UniRx;

namespace SocialGame.Internal.Sound.Unity
{
    internal sealed class UnityVoiceModel : IInitializable, IDisposable, ISoundModel
    {
        [Inject] private IVoiceIntent _intent = null;

        [Inject] private ISoundVolumeIntent _volumeIntent = null;

        [Inject] private UnityVoiceSettings _settings = null;

        private ReactiveCollection<AudioSource> _audioSources = new ReactiveCollection<AudioSource>();

        private readonly CompositeDisposable _disposable = new CompositeDisposable();

        void IInitializable.Initialize()
        {
            if (_settings.Group == null)
                return;
            
            var playIndex = 0;
            _intent.OnPlayAsObservable()
                .Subscribe(voice => {
                    var audioSource =_audioSources[playIndex];
                    var clip = _settings.Clips.ElementAt((int)voice);
                    audioSource.PlayOneShot(clip);
                    playIndex++;
                })
                .AddTo(_disposable);

            _intent.OnPlayForNameAsObservable()
                .Subscribe(_ => Debug.unityLogger.LogWarning(GetType().Name, "not supported"))
                .AddTo(_disposable);
            
            _intent.OnStopAsObservable()
                .Subscribe(_ => _audioSources.ForEach(x => x.Stop()))
                .AddTo(_disposable);

            _volumeIntent.OnVoiceVolumeAsObservable()
                .Select(x => Mathf.Lerp(-80.0F, 0.0F, Mathf.Clamp01(x)))
                .Subscribe(x => _settings.Group.audioMixer.SetFloat(_settings.VolumeExposedParameter, x))
                .AddTo(_disposable);

            for (var i = 0; i < _settings.MaxPlayCount; i++)
            {
                var audioSource = new GameObject($"Voice_{i:000}").AddComponent<AudioSource>();
                audioSource.outputAudioMixerGroup = _settings.Group;
                _audioSources.Add(audioSource);
            }
        }

        void IDisposable.Dispose()
        {
            _disposable.Dispose();
        }

        #region IVoiceModel implementation
        IObservable<Unit> ISoundModel.OnInitializeAsObservable()
        {
            return Observable.ReturnUnit();
        }
        
        IObservable<Transform> ISoundModel.OnAddObjectAsObservable()
        {
            return _audioSources
                .ObserveAdd()
                .Select(x => x.Value.transform);
        }
        #endregion
    }
}