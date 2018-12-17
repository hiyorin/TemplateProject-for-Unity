using System;
using UniRx;

namespace SocialGame.Dialog
{
    public interface IDialogController
    {
        void Clear();
        
        IObservable<Unit> Open(DialogType type, object param = null);
        
        IObservable<TResult> Open<TResult>(DialogType type, object param = null);

        IObservable<Unit> OpenPrimary(DialogType type, object param = null);
        
        IObservable<TResult> OpenPrimary<TResult>(DialogType type, object param = null);
    }
}
