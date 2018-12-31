using System;
using SocialGame.Loading;
using UnityEngine;
using UnityEngine.UI;
using UnityExtensions;
using UniRx;
using DG.Tweening;

namespace SocialGame.Internal.Loading.Builtin
{
    internal sealed class SampleLoading : MonoBehaviour, ILoading
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
            return _canvasGroup
                .DOFade(1.0f, defaultDuration)
                .OnCompleteAsObservable();
        }

        IObservable<Unit> ILoading.OnHideAsObservable(float defaultDuration)
        {
            return _canvasGroup
                .DOFade(0.0f, defaultDuration)
                .OnCompleteAsObservable();
        }
        #endregion
    }
}
