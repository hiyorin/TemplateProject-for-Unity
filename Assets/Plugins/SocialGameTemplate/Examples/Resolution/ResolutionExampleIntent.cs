using System;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

namespace Sandbox.Resolution
{
    internal interface IResolutionExampleIntent
    {
        IObservable<Unit> OnClickLowButtonAsObservable();
        
        IObservable<Unit> OnClickMiddleButtonAsObservable();
        
        IObservable<Unit> OnClickHighButtonAsObservable();
    }
        
     internal sealed class ResolutionExampleIntent : MonoBehaviour, IResolutionExampleIntent
     {
        [SerializeField] private Button _lowButton = null;
        
        [SerializeField] private Button _middleButton = null;
        
        [SerializeField] private Button _highButton = null;
        
        IObservable<Unit> IResolutionExampleIntent.OnClickLowButtonAsObservable()
        {
            return _lowButton.OnClickAsObservable();
        }
        
        IObservable<Unit> IResolutionExampleIntent.OnClickMiddleButtonAsObservable()
        {
            return _middleButton.OnClickAsObservable();
        }
        
        IObservable<Unit> IResolutionExampleIntent.OnClickHighButtonAsObservable()
        {
            return _highButton.OnClickAsObservable();
        }
    }
}
