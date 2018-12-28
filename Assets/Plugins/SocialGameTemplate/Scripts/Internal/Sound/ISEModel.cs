using System;
using UnityEngine;
using UniRx;

namespace SocialGame.Internal.Sound
{
    internal interface ISEModel
    {
        IObservable<Transform> OnAddObjectAsObservable();
    }
}