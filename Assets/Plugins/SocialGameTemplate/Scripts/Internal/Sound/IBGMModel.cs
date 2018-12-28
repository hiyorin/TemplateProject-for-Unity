using System;
using UnityEngine;
using UniRx;

namespace SocialGame.Internal.Sound
{
    internal interface IBGMModel
    {
        IObservable<Transform> OnAddObjectAsObservable();
    }
}