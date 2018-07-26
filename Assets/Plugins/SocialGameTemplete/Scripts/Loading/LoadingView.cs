using System;
using UnityEngine;
using UnityEngine.UI;
using UnityExtensions;
using Zenject;
using UniRx;

namespace SocialGame.Loading
{
    public sealed class LoadingView : MonoBehaviour
    {
        [SerializeField] private Transform _container;

        [SerializeField] private Graphic _mask;

        [Inject] private ILoadingModel _model;

        private void Start()
        {
            _model
                .OnAddAsObservable()
                .Subscribe(x => x.transform.SetParent(_container, false))
                .AddTo(this);

            _model
                .OnShowAsObservable()
                .Subscribe(_ => _mask.gameObject.SetActiveSafe(true))
                .AddTo(this);

            _model
                .OnHideAsObservable()
                .Subscribe(_ => _mask.gameObject.SetActiveSafe(false))
                .AddTo(this);
        }
    }
}