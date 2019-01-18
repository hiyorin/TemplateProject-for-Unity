using SocialGame.Toast;
using UnityEngine;
using UnityEngine.UI;
using UnityExtensions;
using UniRx.Async;
using DG.Tweening;

namespace SocialGame.Internal.Toast.Builtin
{
    internal sealed class SampleToast : MonoBehaviour, IToast
    {
        [SerializeField] private Text _message = null;

        private void Start()
        {
            transform.localScale = Vector3.zero;
        }

        #region IToast implementation
        async UniTask IToast.OnOpen(object param, float defaultDuration)
        {
            _message.text = param as string;
            await transform
                .DOScale(Vector3.one, defaultDuration)
                .OnCompleteAsUniTask();
        }

        async UniTask IToast.OnClose(float defaultDuration)
        {
            await transform
                .DOScale(Vector3.zero, defaultDuration)
                .OnCompleteAsUniTask();
        }
        #endregion
    }
}
