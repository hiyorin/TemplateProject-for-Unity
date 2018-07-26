using System;
using UnityEngine;
using UniRx;

namespace SocialGame.Toast
{
    public interface IToastModel
    {
        IObservable<GameObject> OnAddAsObservable();

        IObservable<Unit> OnOpenAsObservable();

        IObservable<Unit> OnCloseAsObservable();
    }
}
