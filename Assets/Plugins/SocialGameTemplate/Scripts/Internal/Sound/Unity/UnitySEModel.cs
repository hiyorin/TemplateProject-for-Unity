using System;
using System.Linq;
using System.Collections.Generic;
using SocialGame.Sound;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityExtensions;
using Zenject;
using UniRx;
using UniRx.Async;

namespace SocialGame.Internal.Sound.Unity
{
    internal sealed class UnitySEModel : IInitializable, IDisposable, ISoundModel
    {
        [Inject] private ISEIntent _intent = null;

        [Inject] private ISoundVolumeIntent _volumeIntent = null;

        [Inject] private UnitySESettings _settings = null;

        private readonly ReactiveCollection<AudioSource> _sources = new ReactiveCollection<AudioSource>();

        private readonly Queue<AudioClip> _playQueue = new Queue<AudioClip>();

        private readonly Dictionary<string, AudioClip> _clips = new Dictionary<string, AudioClip>();
        
        private readonly CompositeDisposable _disposable = new CompositeDisposable();

        void IInitializable.Initialize()
        {
            if (_settings.Group == null)
                return;

            foreach (SE se in Enum.GetValues(typeof(SE)))
            {
                var clip = _settings.Clips.ElementAt((int)se);
                _clips.Add(se.ToString(), clip);
            }
            
            Observable.Merge(
                    _intent.OnPlayAsObservable().Select(x => x.ToString()),
                    _intent.OnPlayForNameAsObservable())
                .Subscribe(x => Play(x).GetAwaiter())
                .AddTo(_disposable);

            _volumeIntent.OnSEVolumeAsObservable()
                .Select(x => Mathf.Lerp(-80.0F, 0.0F, Mathf.Clamp01(x)))
                .Subscribe(x => _settings.Group.audioMixer.SetFloat(_settings.VolumeExposedParameter, x))
                .AddTo(_disposable);
            
            int playIndex = 0;
            Observable.EveryUpdate()
                .Where(_ => _playQueue.Count > 0)
                .Select(_ => _playQueue.Distinct())
                .Subscribe(x => {
                    x.ForEach(clip => {
                        var audioSource = _sources[playIndex];
                        audioSource.PlayOneShot(clip);
                        if (++playIndex >= _sources.Count)
                            playIndex = 0;
                    });
                    _playQueue.Clear();
                })
                .AddTo(_disposable);
            
            for (var i = 0; i < _settings.MaxPlayCount; i++)
            {
                var audioSource = new GameObject($"SE_{i:000}").AddComponent<AudioSource>();
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
            
            _playQueue.Enqueue(clip);
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
