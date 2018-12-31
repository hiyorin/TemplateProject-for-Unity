using System;
using UniRx;

namespace SocialGame.Loading
{
    public interface ILoading
    {
        IObservable<Unit> OnShowAsObservable(float defaultDuration);

        IObservable<Unit> OnHideAsObservable(float defaultDuration);
    }
}
