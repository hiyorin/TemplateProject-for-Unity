using System;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

namespace Sandbox.Network
{
    internal interface INetworkExampleIntent
    {
        IObservable<Unit> OnClickRequestButtonAsObservable();
    }

    internal sealed class NetworkExampleIntent : MonoBehaviour, INetworkExampleIntent
    {
        [SerializeField] private Button _requestButton = null;
        
        IObservable<Unit> INetworkExampleIntent.OnClickRequestButtonAsObservable()
        {
            return _requestButton.OnClickAsObservable();
        }
    }
}