using System;
using SocialGame.Internal.Dialog;
using UniRx;

namespace SocialGame.Dialog
{
    public sealed class DialogController : IDialogIntent
    {
        private readonly Subject<RequestDialog> _onOpen = new Subject<RequestDialog>();

        private readonly Subject<object> _onClose = new Subject<object>();

        public IObservable<object> Open(DialogType type, object param)
        {
            _onOpen.OnNext(new RequestDialog(type, param));
            return _onClose;
        }

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
