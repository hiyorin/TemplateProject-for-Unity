using System;
using UniRx.Async;

namespace SocialGame.Dialog
{
    public interface IDialog
    {
        /// <summary>
        /// Open interaction
        /// </summary>
        /// <param name="defaultDuration"></param>
        /// <returns></returns>
        UniTask OnOpen(float defaultDuration);

        /// <summary>
        /// Close interaction
        /// </summary>
        /// <param name="defaultDuration"></param>
        /// <returns></returns>
        UniTask OnClose(float defaultDuration);
        
        /// <summary>
        /// Received open param
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        UniTask OnStart(object param);

        /// <summary>
        /// Received resume param
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        UniTask OnResume(object param);

        /// <summary>
        /// Open next dialog.
        /// </summary>
        /// <returns></returns>
        IObservable<RequestDialog> OnNextAsObservable();

        /// <summary>
        /// Open prev dialog.
        /// </summary>
        /// <returns></returns>
        IObservable<object> OnPreviousAsObservable();
    }
}

