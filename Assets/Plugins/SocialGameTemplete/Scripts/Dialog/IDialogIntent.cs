using System;
using UniRx;

namespace SocialGame.Dialog
{
    public interface IDialogIntent
    {
        void Close(object result);
        IObservable<RequestDialog> OnOpenAsObservable();
    }
}
