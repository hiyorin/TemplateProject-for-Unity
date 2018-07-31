using System;
using UnityEngine;
using UniRx;

namespace SocialGame.Internal.Dialog
{
    internal interface IDialogModel
    {
        IObservable<GameObject> OnAddAsObservable();

        IObservable<int> OnOpenAsObservable();

        IObservable<int> OnCloseAsObservable();
    }
}
