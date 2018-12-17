using System;
using SocialGame.Toast;
using UniRx;

namespace SocialGame.Internal.Toast
{
    public sealed class ToastController : IToastIntent, IToastController
    {
        private readonly Subject<RequestToast> _onOpen = new Subject<RequestToast>();

        #region IToastController implementation
        void IToastController.Open(RequestToast request)
        {
            _onOpen.OnNext(request);
        }
        #endregion
        
        #region IToastIntent implementation
        IObservable<RequestToast> IToastIntent.OnOpenAsObservable()
        {
            return _onOpen;
        }
        #endregion
    }
}
