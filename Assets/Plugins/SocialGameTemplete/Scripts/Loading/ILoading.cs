using System;
using UniRx;

namespace SocialGame.Loading
{
    public interface ILoading
    {
        IObservable<Unit> OnShowAsObservable();

        IObservable<Unit> OnHideAsObservable();
    }
}
