using System;
using System.Collections.Generic;
using System.Linq;
using SocialGame.Sound;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityExtensions;
using Zenject;
using UniRx;
using UniRx.Async;

namespace SocialGame.Internal.Sound.Unity
{
    internal sealed class UnityVoiceModel : IInitializable, IDisposable, ISoundModel
    {
        [Inject] private IVoiceIntent _intent = null;

        [Inject] private ISoundVolumeIntent _volumeIntent = null;

        [Inject] private UnityVoiceSettings _settings = null;

        private ReactiveCollection<AudioSource> _sources = new ReactiveCollection<AudioSource>();

        private readonly Dictionary<string, AudioClip> _clips = new Dictionary<string, AudioClip>();
        
        private readonly CompositeDisposable _disposable = new CompositeDisposable();

        void IInitializable.Initialize()
        {
            if (_settings.Group == null)
                return;

            foreach (Voice voice in Enum.GetValues(typeof(Voice)))
            {
                var clip = _settings.Clips.ElementAt((int)voice);
                _clips.Add(voice.ToString(), clip);
            }

            Observable.Merge(
                    _intent.OnPlayAsObservable().Select(x => x.ToString()),
                    _intent.OnPlayForNameAsObservable())
                .Subscribe(x => Play(x).GetAwaiter())
                .AddTo(_disposable);

            _intent.OnStopAsObservable()
                .Subscribe(_ => _sources.ForEach(x => x.Stop()))
                .AddTo(_disposable);

            _volumeIntent.OnVoiceVolumeAsObservable()
                .Select(x => Mathf.Lerp(-80.0F, 0.0F, Mathf.Clamp01(x)))
                .Subscribe(x => _settings.Group.audioMixer.SetFloat(_settings.VolumeExposedParameter, x))
                .AddTo(_disposable);

            for (var i = 0; i < _settings.MaxPlayCount; i++)
            {
                var audioSource = new GameObject($"Voice_{i:000}").AddComponent<AudioSource>();
                audioSource.outputAudioMixerGroup = _settings.Group;
                _sources.Add(audioSource);
            }
        }

        void IDisposable.Dispose()
        {
            _disposable.Dispose();
        }

        private async UniTask Play(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                Debug.unityLogger.LogError(GetType().Name, $"name is null or empty.");
                return;
            }

            AudioClip clip;
            if (_clips.TryGetValue(name, out clip))
            {
                var operation = Addressables.LoadAsset<AudioClip>(name);
                await operation.ToUniTask();
                clip = operation.Result;
            }

            if (clip == null)
            {
                Debug.unityLogger.LogError(GetType().Name, $"{name} is not found.");
                return;
            }

            var source = _sources.FirstOrDefault(x => !x.isPlaying);
            if (source == null)
            {
                Debug.unityLogger.LogWarning(GetType().Name, $"could not play {name}, because of the number of playback.");
                return;
            }
            
            source.PlayOneShot(clip);
        }
        
        #region IVoiceModel implementation
        IObservable<Unit> ISoundModel.OnInitializedAsObservable()
        {
            return Observable.ReturnUnit();
        }
        
        IObservable<Transform> ISoundModel.OnAddObjectAsObservable()
        {
            return _sources
                .ObserveAdd()
                .Select(x => x.Value.transform);
        }
        #endregion
    }
}