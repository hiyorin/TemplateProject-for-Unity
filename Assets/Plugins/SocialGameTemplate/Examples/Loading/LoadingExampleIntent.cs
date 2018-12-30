using System;
using UnityEngine;
using UniRx;
using UnityEngine.UI;

namespace Sandbox.Loading
{
    internal interface ILoadingExampleIntent
    {
        IObservable<Unit> OnClickShowButtonAsObservable();
    }
    
    internal sealed class LoadingExampleIntent : MonoBehaviour, ILoadingExampleIntent
    {
        [SerializeField] private Button _showButton = null;

        IObservable<Unit> ILoadingExampleIntent.OnClickShowButtonAsObservable()
        {
            return _showButton.OnClickAsObservable();
        }
    }
}