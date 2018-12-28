using System;
using UnityEngine;
using Zenject;
using UniRx;

namespace SocialGame.Internal.Sound.Unity
{
    internal sealed class UnityMasterModel : IInitializable, IDisposable
    {
        [Inject] private ISoundVolumeIntent _volumeIntent = null;

        [Inject] private UnityMasterSettings _settings = null;

        private readonly CompositeDisposable _disposable = new CompositeDisposable();

        void IInitializable.Initialize()
        {
            if (_settings.Group == null)
                return;
            
            _volumeIntent
                .OnMasterVolumeAsObservable()
                .Select(x => Mathf.Lerp(-80.0F, 0.0F, Mathf.Clamp01(x)))
                .Subscribe(x => _settings.Group.audioMixer.SetFloat(_settings.VolumeExposedParameter, x))
                .AddTo(_disposable);
        }

        void IDisposable.Dispose()
        {
            _disposable.Dispose();
        }
    }
}
