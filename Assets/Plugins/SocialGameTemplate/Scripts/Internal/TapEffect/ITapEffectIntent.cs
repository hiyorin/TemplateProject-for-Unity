using System;
using SocialGame.TapEffect;
using UniRx;

namespace SocialGame.Internal.TapEffect
{
    internal interface ITapEffectIntent
    {
        IObservable<TapEffectType> OnStartAsObservable();

        IObservable<Unit> OnStopAsObservable();
    }
}
