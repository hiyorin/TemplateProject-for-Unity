using System;
using UnityEngine;
using UniRx;

namespace SocialGame.Dialog
{
    public interface IDialogModel
    {
        IObservable<GameObject> OnAddAsObservable();

        IObservable<int> OnOpenAsObservable();

        IObservable<int> OnCloseAsObservable();
    }
}
