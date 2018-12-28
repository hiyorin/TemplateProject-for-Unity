using System;
using UnityEngine;
using UniRx;

namespace SocialGame.Internal.Sound
{
    internal interface IVoiceModel
    {
        IObservable<Transform> OnAddObjectAsObservable();
    }
}