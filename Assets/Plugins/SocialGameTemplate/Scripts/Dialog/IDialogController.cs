using System;
using UniRx;

namespace SocialGame.Dialog
{
    public interface IDialogController
    {
        IObservable<Unit> Open(DialogType type, object param = null);
        
        IObservable<TResult> Open<TResult>(DialogType type, object param = null);
    }
}
