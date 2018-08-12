using System;
using SocialGame.Internal.Loading;
using UniRx;

namespace SocialGame.Loading
{
    public sealed class LoadingController : ILoadingIntent
    {
        private readonly Subject<LoadingType> _onShow = new Subject<LoadingType>();

        private readonly Subject<Unit> _onHide = new Subject<Unit>();

        public void Show(LoadingType type)
        {
            _onShow.OnNext(type);
        }

        public void Hide()
        {
            _onHide.OnNext(Unit.Default);
        }

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
