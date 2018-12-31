using System;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

namespace Sandbox.Toast
{
    internal interface IToastExampleIntent
    {
        IObservable<Unit> OnClickShowButtonAsObservable();
    }
    
    internal sealed class ToastExampleIntent : MonoBehaviour, IToastExampleIntent
    {
        [SerializeField] private Button _showButton = null;
        
        IObservable<Unit> IToastExampleIntent.OnClickShowButtonAsObservable()
        {
            return _showButton.OnClickAsObservable();
        }
    }
}