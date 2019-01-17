using System;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.UI;

namespace SocialGame.Examples.Sound
{
    internal interface ISoundExampleIntent
    {
        IObservable<float> OnVolumeAsObservable();
        
        IObservable<string> OnSelectAsObservable();

        IObservable<Unit> OnClickPlayButtonAsObservable();
        
        IObservable<Unit> OnClickPauseButtonAsObservable();
        
        IObservable<Unit> OnClickStopButtonAsObservable();
    }
    
    internal sealed class SoundExampleIntent : MonoBehaviour, ISoundExampleIntent
    {
        [SerializeField] private Slider _volumeSlider = null;
        
        [SerializeField] private Dropdown _dropdown = null;

        [SerializeField] private Button _playButton = null;

        [SerializeField] private Button _pauseButton = null;

        [SerializeField] private Button _stopButton = null;

        IObservable<float> ISoundExampleIntent.OnVolumeAsObservable()
        {
            return _volumeSlider.OnValueChangedAsObservable();
        }
        
        IObservable<string> ISoundExampleIntent.OnSelectAsObservable()
        {
            return _dropdown.OnValueChangedAsObservable()
                .Where(x => _dropdown.options.Count > 0)
                .Select(x => _dropdown.options[x].text);
        }
        
        IObservable<Unit> ISoundExampleIntent.OnClickPlayButtonAsObservable()
        {
            return _playButton.OnClickAsObservable();
        }
        
        IObservable<Unit> ISoundExampleIntent.OnClickPauseButtonAsObservable()
        {
            return _pauseButton.OnClickAsObservable();
        }
        IObservable<Unit> ISoundExampleIntent.OnClickStopButtonAsObservable()
        {
            return _stopButton.OnClickAsObservable();
        }
    }
}