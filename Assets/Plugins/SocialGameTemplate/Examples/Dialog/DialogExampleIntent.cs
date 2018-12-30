using System;
using UnityEngine;
using UniRx;
using UnityEngine.UI;

namespace Sandbox.Dialog
{
    internal interface IDialogExampleIntent
    {
        IObservable<Unit> OnClickOpenButtonAsObservable();

        IObservable<Unit> OnClickOpenPrimaryButtonAsObservable();
    }

    internal sealed class DialogExampleIntent : MonoBehaviour, IDialogExampleIntent
    {
        [SerializeField] private Button _openButton = null;

        [SerializeField] private Button _openPrimaryButton = null;

        IObservable<Unit> IDialogExampleIntent.OnClickOpenButtonAsObservable()
        {
            return _openButton.OnClickAsObservable();
        }

        IObservable<Unit> IDialogExampleIntent.OnClickOpenPrimaryButtonAsObservable()
        {
            return _openPrimaryButton.OnClickAsObservable();
        }
    }
}
