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
    internal sealed class UnityBGMModel : IInitializable, IDisposable, ISoundModel
    {
        [Inject] private IBGMIntent _intent = null;

        [Inject] private ISoundVolumeIntent _volumeIntent = null;

        [Inject] private UnityBGMSettings _settings = null;

        private readonly ReactiveCollection<AudioSource> _sources = new ReactiveCollection<AudioSource>();

        private readonly Dictionary<string, AudioClip> _clips = new Dictionary<string,AudioClip>();
        
        private readonly CompositeDisposable _disposable = new CompositeDisposable();

        void IInitializable.Initialize()
        {
            if (_settings.Group == null)
                return;

            foreach (BGM bgm in Enum.GetValues(typeof(BGM)))
            {
                var clip = _settings.Clips.ElementAt((int)bgm);
                _clips.Add(bgm.ToString(), clip);
            }
            
            Observable.Merge(
                    _intent.OnPlayAsObservable().Select(x => x.ToString()),
                    _intent.OnPlayForNameAsObservable())
                .Subscribe(x => Play(x).GetAwaiter())
                .AddTo(_disposable);

            _intent.OnStopAsObservable()
                .SelectMany(_ => _sources
                    .Select(x => x.FadeOutAsCoroutine(_settings.FadeOutDuration).ToObservable())
                    .WhenAll()
                    .First())
                .Subscribe(_ => {
                    _sources.ForEach(x => {
                        x.Stop();
                        x.enabled = false;
                    });
                })
                .AddTo(_disposable);
            
            _volumeIntent.OnBGMVolumeAsObservable()
                .Select(x => Mathf.Lerp(-80.0F, 0.0F, Mathf.Clamp01(x)))
                .Subscribe(x => _settings.Group.audioMixer.SetFloat(_settings.VolumeExposedParameter, x))
                .AddTo(_disposable);

            for (var i = 0; i < 2; i++)
            {
                var audioSource = new GameObject($"BGM_{i:000}").AddComponent<AudioSource>();
                audioSource.outputAudioMixerGroup = _settings.Group;
                audioSource.enabled = false;
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
                Debug.unityLogger.LogError(GetType().Name, $"name is null or empty");
                return;
            }
            
            var afterSource = _sources.FirstOrDefault(x => !x.enabled);
            if (afterSource == null)
            {
                Debug.unityLogger.LogWarning(GetType().Name, $"could not play {name}, because of the number of playback.");
                return;
            }

            AudioClip clip;
            if (!_clips.TryGetValue(name, out clip))
            {
                var operation = Addressables.LoadAsset<AudioClip>(name);
                await operation.ToUniTask();
                clip = operation.Result;
                _clips.Add(name, clip);
            }

            if (clip == null)
            {
                Debug.unityLogger.LogError(GetType().Name, $"{name} is not found.");
                return;
            }
            
            var beforeSource = _sources.FirstOrDefault(x => x.enabled);
            afterSource.enabled = true;
            afterSource.PlayWithFadeIn(clip, 1.0f, _settings.FadeInDuration);

            if (beforeSource == null)
                return;
            await beforeSource.FadeOutAsCoroutine(_settings.FadeOutDuration).ToUniTask();
            beforeSource.Stop();
            beforeSource.enabled = false;
        }

        #region ISoundModel implementation
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
