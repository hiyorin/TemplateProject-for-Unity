using System;
using UniRx;

namespace SocialGame.Dialog
{
    public interface IDialog
    {
        IObservable<Unit> OnOpenAsObservable(float defaultDuration);

        IObservable<Unit> OnCloseAsObservable(float defaultDuration);

        IObservable<Unit> OnStartAsObservable(object param);

        IObservable<Unit> OnResumeAsObservable(object param);

        IObservable<RequestDialog> OnNextAsObservable();

        IObservable<object> OnPreviousAsObservable();
    }
}

