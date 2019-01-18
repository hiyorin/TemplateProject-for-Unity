using SocialGame.Transition;
using UnityEngine;
using UnityEngine.UI;
using UnityExtensions;
using UniRx.Async;
using DG.Tweening;

namespace SocialGame.Internal.Transition.Builtin
{
    internal sealed class BlackFadeTransition : MonoBehaviour, ITransition
    {
        [SerializeField] private Graphic _background = null;

        #region ITransition implementation
        async UniTask ITransition.OnTransIn(float defaultDuration)
        {
            await _background
                .DOFade(1.0f, defaultDuration)
                .OnCompleteAsUniTask();
        }

        async UniTask ITransition.OnTransOut(float defaultDuration)
        {
            await _background
                .DOFade(0.0f, defaultDuration)
                .OnCompleteAsUniTask();
        }
        #endregion
    }
}
