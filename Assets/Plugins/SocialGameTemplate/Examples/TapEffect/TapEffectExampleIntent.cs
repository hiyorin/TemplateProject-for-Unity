using System;
using UnityEngine;
using UniRx;
using UnityEngine.UI;

namespace Sandbox.TapEffect
{
    internal interface ITapEffectExampleIntent
    {
        IObservable<Unit> OnClickShowButtonAsObservable();
        
        IObservable<Unit> OnClickHideButtonAsObservable();
    }
    
    internal sealed class TapEffectExampleIntent : MonoBehaviour, ITapEffectExampleIntent
    {
        [SerializeField] private Button _showButton = null;
        
        [SerializeField] private Button _hideButton = null;
        
        IObservable<Unit> ITapEffectExampleIntent.OnClickShowButtonAsObservable()
        {
            return _showButton.OnClickAsObservable();
        }

        IObservable<Unit> ITapEffectExampleIntent.OnClickHideButtonAsObservable()
        {
            return _hideButton.OnClickAsObservable();
        }
    }
}