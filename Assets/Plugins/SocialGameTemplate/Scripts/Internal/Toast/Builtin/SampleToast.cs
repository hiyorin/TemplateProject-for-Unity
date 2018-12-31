using System;
using SocialGame.Toast;
using UnityEngine;
using UnityEngine.UI;
using UnityExtensions;
using UniRx;
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
        IObservable<Unit> IToast.OnOpenAsObservable(object param, float defaultDuration)
        {
            _message.text = param as string;
            return transform
                .DOScale(Vector3.one, defaultDuration)
                .OnCompleteAsObservable();
        }

        IObservable<Unit> IToast.OnCloseAsObservable(float defaultDuration)
        {
            return transform
                .DOScale(Vector3.zero, defaultDuration)
                .OnCompleteAsObservable();
        }
        #endregion
    }
}
