using System;
using SocialGame.Toast;
using SocialGame.Internal.Toast;
using UniRx;

namespace SocialGame
{
    public sealed class ToastController : IToastIntent
    {
        private readonly Subject<RequestToast> _onOpen = new Subject<RequestToast>();

        public void Open(RequestToast request)
        {
            _onOpen.OnNext(request);
        }

        #region IToastIntent implementation
        IObservable<RequestToast> IToastIntent.OnOpenAsObservable()
        {
            return _onOpen;
        }
        #endregion
    }
}
