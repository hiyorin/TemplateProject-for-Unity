using System;
using UniRx;

namespace SocialGame.Loading
{
    public interface ILoadingIntent
    {
        IObservable<LoadingType> OnShowAsObservable();

        IObservable<Unit> OnHideAsObservable();
    }
}
