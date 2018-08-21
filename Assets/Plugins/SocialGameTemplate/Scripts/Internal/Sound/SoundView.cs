using System;
using UnityEngine;
using Zenject;
using UniRx;

namespace SocialGame.Internal.Sound
{
    internal sealed class SoundView : MonoBehaviour
    {
        [SerializeField] private Transform _bgmContainer = null;

        [SerializeField] private Transform _seContainer = null;

        [SerializeField] private Transform _voiceContainer = null;

        [Inject] private IBGMModel _bgmModel = null;

        [Inject] private ISEModel _seModel = null;

        [Inject] private IVoiceModel _voiceModel = null;

        private void Start()
        {
            _bgmModel
                .OnAddAudioSourceAsObservable()
                .Subscribe(x => x.transform.parent = _bgmContainer)
                .AddTo(this);

            _seModel
                .OnAddAudioSourceAsObservable()
                .Subscribe(x => x.transform.parent = _seContainer)
                .AddTo(this);

            _voiceModel
                .OnAddAudioSourceAsObservable()
                .Subscribe(x => x.transform.parent = _voiceContainer)
                .AddTo(this);
        }
    }
}
