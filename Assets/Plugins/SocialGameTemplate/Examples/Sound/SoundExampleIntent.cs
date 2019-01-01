using System;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.UI;

namespace SocialGame.Examples.Sound
{
    internal interface ISoundExampleIntent
    {
        IObservable<string> OnSelectBgmAsObservable();
    }
    
    internal sealed class SoundExampleIntent : MonoBehaviour, ISoundExampleIntent
    {
        [SerializeField] private Dropdown _bgmDropdown = null;

        IObservable<string> ISoundExampleIntent.OnSelectBgmAsObservable()
        {
            return _bgmDropdown.OnSubmitAsObservable()
                .Select(x => x.selectedObject.name);
        }
    }
}