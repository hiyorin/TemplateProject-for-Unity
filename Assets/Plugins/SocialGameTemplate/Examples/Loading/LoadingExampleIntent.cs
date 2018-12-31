using System;
using UnityEngine;
using UniRx;
using UnityEngine.UI;

namespace Sandbox.Loading
{
    internal interface ILoadingExampleIntent
    {
        IObservable<Unit> OnClickShowSampleButtonAsObservable();

        IObservable<Unit> OnClickShowSystemButtonAsObservable();
    }
    
    internal sealed class LoadingExampleIntent : MonoBehaviour, ILoadingExampleIntent
    {
        [SerializeField] private Button _showSampleButton = null;

        [SerializeField] private Button _showSystemButton = null;

        IObservable<Unit> ILoadingExampleIntent.OnClickShowSampleButtonAsObservable()
        {
            return _showSampleButton.OnClickAsObservable();
        }

        IObservable<Unit> ILoadingExampleIntent.OnClickShowSystemButtonAsObservable()
        {
            return _showSystemButton.OnClickAsObservable();
        }
    }
}