using System;
using UniRx;
using UniRx.Async;

namespace SocialGame.Dialog
{
    public interface IDialog
    {
        UniTask OnOpen(float defaultDuration);

        UniTask OnClose(float defaultDuration);
        
        UniTask OnStart(object param);

        UniTask OnResume(object param);

        IObservable<RequestDialog> OnNextAsObservable();

        IObservable<object> OnPreviousAsObservable();
    }
}

