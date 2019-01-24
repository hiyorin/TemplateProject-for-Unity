using System;
using SocialGame.Loading;
using UniRx;

namespace SocialGame.Internal.Loading
{
    internal sealed class LoadingController : ILoadingController, ILoadingIntent
    {
        private readonly Subject<string> _onShow = new Subject<string>();

        private readonly Subject<Unit> _onHide = new Subject<Unit>();

        #region ILoadingController implementation
        void ILoadingController.Show(LoadingType type)
        {
            _onShow.OnNext(type.ToString());
        }

        void ILoadingController.Show(string name)
        {
            _onShow.OnNext(name);
        }
        
        void ILoadingController.Hide()
        {
            _onHide.OnNext(Unit.Default);
        }
        #endregion
        
        #region ILoadingIntent implementation
        IObservable<string> ILoadingIntent.OnShowAsObservable()
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
