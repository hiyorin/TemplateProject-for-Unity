using System;
using UnityEngine;
using UniRx;

namespace SocialGame.Internal.Loading
{
    internal interface ILoadingModel
    {
        IObservable<GameObject> OnAddAsObservable();

        IObservable<Unit> OnShowAsObservable();

        IObservable<Unit> OnHideAsObservable();
    }
}
