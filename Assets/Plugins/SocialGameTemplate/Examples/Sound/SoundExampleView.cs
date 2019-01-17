using System.Linq;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace SocialGame.Examples.Sound
{
    internal sealed class SoundExampleView : MonoBehaviour
    {
        [SerializeField] private Slider _bgmVolume = null;
        
        [SerializeField] private Slider _seVolume = null;
        
        [SerializeField] private Slider _voiceVolume = null;
        
        [SerializeField] private Dropdown _bgmDropdown = null;

        [SerializeField] private Dropdown _seDropdown = null;
        
        [SerializeField] private Dropdown _voiceDropdown = null;
        
        [Inject] private ISoundExampleModel _model = null;

        private void Awake()
        {
            _bgmDropdown.ClearOptions();
            _seDropdown.ClearOptions();
            _voiceDropdown.ClearOptions();
        }
        
        private void Start()
        {
            _model.OnAddBgmAsObservable()
                .Subscribe(x => _bgmDropdown.AddOptions(x.ToList()))
                .AddTo(this);

            _model.OnAddSeAsObservable()
                .Subscribe(x => _seDropdown.AddOptions(x.ToList()))
                .AddTo(this);

            _model.OnAddVoiceAsObservable()
                .Subscribe(x => _voiceDropdown.AddOptions(x.ToList()))
                .AddTo(this);

            _model.OnChangeBgmVolumeAsObservable()
                .Subscribe(x => _bgmVolume.value = x)
                .AddTo(this);
            
            _model.OnChangeSeVolumeAsObservable()
                .Subscribe(x => _seVolume.value = x)
                .AddTo(this);
            
            _model.OnChangeVoiceVolumeAsObservable()
                .Subscribe(x => _voiceVolume.value = x)
                .AddTo(this);
        }
    }
}