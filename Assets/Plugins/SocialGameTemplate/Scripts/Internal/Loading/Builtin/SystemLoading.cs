using System;
using SocialGame.Loading;
using UnityEngine;
using UnityEngine.UI;
using UnityExtensions;
using UniRx;
using DG.Tweening;

namespace SocialGame.Internal.Loading.Builtin
{
    internal sealed class SystemLoading : MonoBehaviour, ILoading
    {
        [SerializeField] private CanvasGroup _canvasGroup = null;

        [SerializeField] private Graphic _icon = null;
        
        private Tween _tween;
        
        private void Start()
        {
            _tween = _icon.transform
                .DORotate(new Vector3(0.0f, 0.0f, 360.0f), 1.5f, RotateMode.LocalAxisAdd)
                .SetLoops(-1, LoopType.Incremental);
            _tween.Play();
        }

        private void OnDestroy()
        {
            if (_tween != null)
            {
                _tween.Kill();
                _tween = null;
            }
        }
        
        #region ILoading implementation
        IObservable<Unit> ILoading.OnShowAsObservable(float defaultDuration)
        {
#if !UNITY_EDITOR && UNITY_ANDROID
                    Handheld.SetActivityIndicatorStyle(AndroidActivityIndicatorStyle.Small);
                    Handheld.StartActivityIndicator();
                    return Observable.ReturnUnit();
#elif !UNITY_EDITOR && UNITY_IOS
                    Handheld.SetActivityIndicatorStyle(UnityEngine.iOS.ActivityIndicatorStyle.White);
                    Handheld.StartActivityIndicator();
                    return Observable.ReturnUnit();
#else
                    return _canvasGroup
                        .DOFade(1.0f, defaultDuration)
                        .OnCompleteAsObservable();
#endif
        }

        IObservable<Unit> ILoading.OnHideAsObservable(float defaultDuration)
        {
#if !UNITY_EDITOR && (UNITY_ANDROID || UNITY_IOS)
            Handheld.StopActivityIndicator();
            return Observable.ReturnUnit();
#else
            return _canvasGroup
                .DOFade(0.0f, defaultDuration)
                .OnCompleteAsObservable();
#endif
        }
        #endregion
    }
}