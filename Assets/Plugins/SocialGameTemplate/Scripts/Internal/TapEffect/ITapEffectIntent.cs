using System;
using SocialGame.TapEffect;
using UniRx;

namespace SocialGame.Internal.TapEffect
{
    internal interface ITapEffectIntent
    {
        IObservable<string> OnStartAsObservable();

        IObservable<Unit> OnStopAsObservable();
    }
}
