using System;
using UnityEngine;
using UniRx;

namespace SocialGame.TapEffect
{
    public interface ITapEffect
    {
        void OnMove(Vector3 position);

        IObservable<Unit> OnShowAsObservable();

        IObservable<Unit> OnHideAsObservable();
    }
}
