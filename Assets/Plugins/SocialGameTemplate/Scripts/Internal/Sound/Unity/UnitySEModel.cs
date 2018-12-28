using System;
using System.Linq;
using System.Collections.Generic;
using SocialGame.Sound;
using UnityEngine;
using UnityExtensions;
using Zenject;
using UniRx;

namespace SocialGame.Internal.Sound.Unity
{
    internal sealed class UnitySEModel : IInitializable, IDisposable, ISoundModel
    {
        [Inject] private ISEIntent _intent = null;

        [Inject] private ISoundVolumeIntent _volumeIntent = null;

        [Inject] private UnitySESettings _settings = null;

        private readonly ReactiveCollection<AudioSource> _audioSources = new ReactiveCollection<AudioSource>();

        private readonly Queue<SE> _playQueue = new Queue<SE>();

        private readonly CompositeDisposable _disposable = new CompositeDisposable();

        void IInitializable.Initialize()
        {
            if (_settings.Group == null)
                return;
            
            _intent
                .OnPlayAsObservable()
                .Subscribe(x => _playQueue.Enqueue(x))
                .AddTo(_disposable);
            
            _volumeIntent
                .OnSEVolumeAsObservable()
                .Select(x => Mathf.Lerp(-80.0F, 0.0F, Mathf.Clamp01(x)))
                .Subscribe(x => _settings.Group.audioMixer.SetFloat(_settings.VolumeExposedParameter, x))
                .AddTo(_disposable);

            int playIndex = 0;
            Observable
                .EveryUpdate()
                .Where(_ => _playQueue.Count > 0)
                .Select(_ => _playQueue.Distinct())
                .Subscribe(x => {
                    x.ForEach(se => {
                        var audioSource = _audioSources[playIndex];
                        var clip = _settings.Clips.ElementAt((int)se);
                        audioSource.PlayOneShot(clip);
                        if (++playIndex >= _audioSources.Count)
                            playIndex = 0;
                    });
                    _playQueue.Clear();
                })
                .AddTo(_disposable);
            
            for (var i = 0; i < _settings.MaxPlayCount; i++)
            {
                var audioSource = new GameObject($"SE_{i:000}").AddComponent<AudioSource>();
                audioSource.outputAudioMixerGroup = _settings.Group;
                _audioSources.Add(audioSource);
            }
        }

        void IDisposable.Dispose()
        {
            _disposable.Dispose();
        }

        #region ISoundModel implementation
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
