using System;
using UnityEngine;
using UnityEngine.UI;
using UnityExtensions;
using Zenject;
using UniRx;
using DG.Tweening;

namespace SocialGame.Dialog
{
    public sealed class SampleDialog : MonoBehaviour, IDialog
    {
        [SerializeField] private Text _message = null;

        [SerializeField] private Button _okButton = null;

        [Inject] private DialogSettings _settings = null;

        private void Start()
        {
            transform.localScale = Vector3.zero;
        }

        #region IDialog implementation
        IObservable<Unit> IDialog.OnOpenAsObservable()
        {
            return transform
                .DOScale(Vector3.one, _settings.DefaoutDuration)
                .OnCompleteAsObservable();
        }

        IObservable<Unit> IDialog.OnCloseAsObservable()
        {
            return transform
                .DOScale(Vector3.zero, _settings.DefaoutDuration)
                .OnCompleteAsObservable();
        }

        IObservable<Unit> IDialog.OnStartAsObservable(object param)
        {
            _message.text = param as string;
            return Observable.ReturnUnit();
        }

        IObservable<Unit> IDialog.OnResumeAsObservable(object param)
        {
            return Observable.ReturnUnit();
        }

        IObservable<RequestDialog> IDialog.OnNextAsObservable()
        {
            return Observable.Empty<RequestDialog>();
        }

        IObservable<object> IDialog.OnPreviousAsObservable()
        {
            return _okButton
                .OnClickAsObservable()
                .Select(_ => "OK");
        }
        #endregion
    }
}
