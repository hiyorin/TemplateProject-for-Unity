using System;
using UnityEngine;
using UniRx;

namespace SocialGame.Internal.Sound
{
    public interface ISoundModel
    {
        IObservable<Unit> OnInitializedAsObservable();
        
        IObservable<Transform> OnAddObjectAsObservable();
    }
}