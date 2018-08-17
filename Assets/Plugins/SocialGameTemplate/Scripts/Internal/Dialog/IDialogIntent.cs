using System;
using SocialGame.Dialog;
using UniRx;

namespace SocialGame.Internal.Dialog
{
    internal interface IDialogIntent
    {
        void Close(object result);
        IObservable<RequestDialog> OnOpenAsObservable();
    }
}
