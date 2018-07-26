using System;
using UnityEngine;
using UnityEngine.UI;
using UnityExtensions;
using Zenject;
using UniRx;
using DG.Tweening;

namespace SocialGame.Toast
{
    public sealed class SampleToast : MonoBehaviour, IToast
    {
        [SerializeField] private Text _message = null;

        [Inject] private ToastSettings _settins = null;

        private void Start()
        {
            transform.localScale = Vector3.zero;
        }

        #region IToast implementation
        IObservable<Unit> IToast.OnOpenAsObservable(object param)
        {
            _message.text = param as string;
            return transform
                .DOScale(Vector3.one, _settins.DefaoutDuration)
                .OnCompleteAsObservable();
        }

        IObservable<Unit> IToast.OnCloseAsObservable()
        {
            return transform
                .DOScale(Vector3.zero, _settins.DefaoutDuration)
                .OnCompleteAsObservable();
        }
        #endregion
    }
}
