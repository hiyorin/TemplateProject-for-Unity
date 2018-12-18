using System;
using SocialGame.Loading;
using UniRx;

namespace SocialGame.Internal.Loading
{
    internal sealed class LoadingController : ILoadingController, ILoadingIntent
    {
        private readonly Subject<LoadingType> _onShow = new Subject<LoadingType>();

        private readonly Subject<Unit> _onHide = new Subject<Unit>();

        #region ILoadingController implementation
        void ILoadingController.Show(LoadingType type)
        {
            _onShow.OnNext(type);
        }

        void ILoadingController.Hide()
        {
            _onHide.OnNext(Unit.Default);
        }
        #endregion
        
        #region ILoadingIntent implementation
        IObservable<LoadingType> ILoadingIntent.OnShowAsObservable()
        {
            return _onShow;
        }

        IObservable<Unit> ILoadingIntent.OnHideAsObservable()
        {
            return _onHide;
        }
        #endregion
    }
}
