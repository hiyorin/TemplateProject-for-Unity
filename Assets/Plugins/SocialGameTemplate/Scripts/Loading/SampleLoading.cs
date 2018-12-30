using System;
using UnityEngine;
using UnityEngine.UI;
using UnityExtensions;
using UniRx;
using DG.Tweening;

namespace SocialGame.Loading
{
    public sealed class SampleLoading : MonoBehaviour, ILoading
    {
        [SerializeField] private CanvasGroup _canvasGroup = null;

        [SerializeField] private Image _icon = null;

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
            _tween.Kill();
            _tween = null;
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
