using System;
using SocialGame.Loading;
using UniRx;

namespace SocialGame.Internal.Loading
{
    internal interface ILoadingIntent
    {
        IObservable<LoadingType> OnShowAsObservable();

        IObservable<Unit> OnHideAsObservable();
    }
}
