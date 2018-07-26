using System;
using UniRx;

namespace SocialGame.TapEffect
{
    public interface ITapEffectIntent
    {
        IObservable<TapEffectType> OnStartAsObservable();

        IObservable<Unit> OnStopAsObservable();
    }
}
