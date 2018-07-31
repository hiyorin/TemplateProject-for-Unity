using System;
using UnityEngine;
using UniRx;

namespace SocialGame.Internal.Toast
{
    internal interface IToastModel
    {
        IObservable<GameObject> OnAddAsObservable();

        IObservable<Unit> OnOpenAsObservable();

        IObservable<Unit> OnCloseAsObservable();
    }
}
