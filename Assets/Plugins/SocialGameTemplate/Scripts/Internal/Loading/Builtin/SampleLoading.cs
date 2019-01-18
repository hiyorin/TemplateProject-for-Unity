using SocialGame.Loading;
using UnityEngine;
using UnityEngine.UI;
using UnityExtensions;
using UniRx.Async;
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
        async UniTask ILoading.OnShow(float defaultDuration)
        {
            await _canvasGroup
                .DOFade(1.0f, defaultDuration)
                .OnCompleteAsUniTask();
        }

        async UniTask ILoading.OnHide(float defaultDuration)
        {
            await _canvasGroup
                .DOFade(0.0f, defaultDuration)
                .OnCompleteAsUniTask();
        }
        #endregion
    }
}
