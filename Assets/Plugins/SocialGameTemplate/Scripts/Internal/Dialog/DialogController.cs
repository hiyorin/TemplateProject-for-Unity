using System;
using SocialGame.Dialog;
using UniRx;

namespace SocialGame.Internal.Dialog
{
    internal sealed class DialogController : IDialogIntent, IDialogController
    {
        private readonly Subject<RequestDialog> _onOpen = new Subject<RequestDialog>();

        private readonly Subject<object> _onClose = new Subject<object>();

        #region IDialogController implementation

        IObservable<Unit> IDialogController.Open(DialogType type, object param)
        {
            _onOpen.OnNext(new RequestDialog(type, param));
            return _onClose
                .First()
                .AsUnitObservable();
        }
        
        IObservable<TResult> IDialogController.Open<TResult>(DialogType type, object param)
        {
            _onOpen.OnNext(new RequestDialog(type, param));
            return _onClose
                .First()
                .Cast<object, TResult>();
        }
        
        #endregion

        #region IDialogIntent implementation
        
        void IDialogIntent.Close(object result)
        {
            _onClose.OnNext(result);
        }

        IObservable<RequestDialog> IDialogIntent.OnOpenAsObservable()
        {
            return _onOpen;
        }
        
        #endregion
    }
}
