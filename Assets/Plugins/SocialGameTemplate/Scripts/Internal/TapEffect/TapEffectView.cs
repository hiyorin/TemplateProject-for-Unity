using System;
using UnityEngine;
using Zenject;
using UniRx;

namespace SocialGame.Internal.TapEffect
{
    internal sealed class TapEffectView : MonoBehaviour
    {
        [SerializeField] private Transform _container = null;

        [Inject] private ITapEffectModel _model = null;

        private void Start()
        {
            _model
                .OnAddAsObservable()
                .Subscribe(x => x.transform.SetParent(_container, false))
                .AddTo(this);
        }
    }
}
