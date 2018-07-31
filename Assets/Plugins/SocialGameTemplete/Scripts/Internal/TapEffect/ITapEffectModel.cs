using System;
using UnityEngine;
using UniRx;

namespace SocialGame.Internal.TapEffect
{
    internal interface ITapEffectModel
    {
        IObservable<GameObject> OnAddAsObservable();
    }
}
