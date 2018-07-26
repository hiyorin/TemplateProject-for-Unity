using System;
using UnityEngine;
using UniRx;

namespace SocialGame.Loading
{
    public interface ILoadingModel
    {
        IObservable<GameObject> OnAddAsObservable();

        IObservable<Unit> OnShowAsObservable();

        IObservable<Unit> OnHideAsObservable();
    }
}
