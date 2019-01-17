using System;
using SocialGame.Dialog;
using UnityEngine;
using UnityEngine.UI;
using UnityExtensions;
using UniRx;
using UniRx.Async;
using DG.Tweening;

namespace SocialGame.Internal.Dialog.Builtin
{
    internal sealed class SampleDialog : MonoBehaviour, IDialog
    {
        [SerializeField] private Text _message = null;

        [SerializeField] private Button _okButton = null;

        private void Start()
        {
            transform.localScale = Vector3.zero;
        }

        #region IDialog implementation
        async UniTask IDialog.OnOpen(float defaultDuration)
        {
            await transform
                .DOScale(Vector3.one, defaultDuration)
                .OnCompleteAsObservable()
                .First();
        }
        
        async UniTask IDialog.OnClose(float defaultDuration)
        {
            await transform
                .DOScale(Vector3.zero, defaultDuration)
                .OnCompleteAsUniTask();
        }

        async UniTask IDialog.OnStart(object param)
        {
            _message.text = param as string;
        }

        async UniTask IDialog.OnResume(object param)
        {
            
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
