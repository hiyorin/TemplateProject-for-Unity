using System;
using SocialGame.Toast;
using UnityEngine;
using UnityEngine.UI;
using UnityExtensions;
using Zenject;
using UniRx;
using DG.Tweening;

namespace SocialGame.Internal.Toast
{
    internal sealed class ToastView : MonoBehaviour
    {
        [SerializeField] private Transform _container = null;

        [SerializeField] private Graphic _mask = null;

        [Inject] private IToastModel _model = null;

        [Inject] private ToastSettings _settings = null;

        private void Start()
        {
            _mask.color = _settings.MaskColor;

            _model.OnAddAsObservable()
                .Subscribe(x => x.transform.SetParent(_container, false))
                .AddTo(this);

            _model.OnOpenAsObservable()
                .Do(_ => _mask.gameObject.SetActiveSafe(true))
                .SelectMany(_ => _mask.DOFade(_settings.MaskColor.a, _settings.DefaoutDuration).OnCompleteAsObservable())
                .Subscribe()
                .AddTo(this);
            
            _model.OnCloseAsObservable()
                .SelectMany(_ => _mask.DOFade(0.0f, _settings.DefaoutDuration).OnCompleteAsObservable())
                .Subscribe(_ => _mask.gameObject.SetActive(false))
                .AddTo(this);
        }
    }
}
