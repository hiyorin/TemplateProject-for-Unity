using System;
using System.Linq;
using SocialGame.Sound;
using UnityEngine;
using UnityExtensions;
using Zenject;
using UniRx;

namespace SocialGame.Internal.Sound.Unity
{
    internal sealed class UnityBGMModel : IInitializable, IDisposable, ISoundModel
    {
        [Inject] private IBGMIntent _intent = null;

        [Inject] private ISoundVolumeIntent _volumeIntent = null;

        [Inject] private UnityBGMSettings _settings = null;

        private ReactiveCollection<AudioSource> _audioSources = new ReactiveCollection<AudioSource>();

        private readonly CompositeDisposable _disposable = new CompositeDisposable();

        void IInitializable.Initialize()
        {
            if (_settings.Group == null)
                return;
            
            _intent.OnPlayAsObservable()
                .SelectMany(bgm => {
                    var afterSource = _audioSources.FirstOrDefault(x => !x.enabled);
                    if (afterSource == null)
                        return Observable.ReturnUnit();

                    var clip = _settings.Clips.ElementAt((int)bgm);
                    var beforeSource = _audioSources.FirstOrDefault(x => x.enabled);
                    afterSource.enabled = true;
                    afterSource.PlayWithFadeIn(clip, 1.0f, _settings.FadeInDuration);
                    if (beforeSource == null)
                        return Observable.ReturnUnit();
                    else
                        return beforeSource
                            .FadeOutAsCoroutine(_settings.FadeOutDuration)
                            .ToObservable()
                            .Do(_ => {
                                beforeSource.Stop();
                                beforeSource.enabled = false;
                            });
                })
                .Subscribe()
                .AddTo(_disposable);

            _intent.OnPlayForNameAsObservable()
                .Subscribe(_ => Debug.unityLogger.LogWarning(GetType().Name, "Not supported"))
                .AddTo(_disposable);
            
            _intent.OnStopAsObservable()
                .SelectMany(_ => _audioSources
                    .Select(x => x.FadeOutAsCoroutine(_settings.FadeOutDuration).ToObservable())
                    .WhenAll()
                    .First())
                .Subscribe(_ => {
                    _audioSources.ForEach(x => {
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
                _audioSources.Add(audioSource);
            }
        }

        void IDisposable.Dispose()
        {
            _disposable.Dispose();
        }

        #region ISoundModel implementation
        IObservable<Unit> ISoundModel.OnInitializedAsObservable()
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
