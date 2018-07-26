using System;
using UnityEngine;
using UniRx;

namespace SocialGame.TapEffect
{
    public interface ITapEffectModel
    {
        IObservable<GameObject> OnAddAsObservable();
    }
}
