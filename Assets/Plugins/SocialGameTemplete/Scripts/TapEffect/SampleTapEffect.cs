using System;
using UnityEngine;
using Zenject;
using UniRx;

namespace SocialGame.TapEffect
{
    public sealed class SampleTapEffect : MonoBehaviour, ITapEffect
    {
        [Inject] private Camera _uiCamera = null;

        [SerializeField] ParticleSystem _particle = null;

        private RectTransform _transform;

        private void Awake()
        {
            _transform = GetComponent<RectTransform>();
        }

        private void Start()
        {
            _particle.Stop();
        }

        #region ITapEffect implementation
        void ITapEffect.OnMove(Vector3 position)
        {
            Vector3 worldPosition = Vector3.zero;
            RectTransformUtility.ScreenPointToWorldPointInRectangle(_transform, position, _uiCamera, out worldPosition);
            _transform.position = worldPosition;
        }

        IObservable<Unit> ITapEffect.OnShowAsObservable()
        {
            return Observable
                .ReturnUnit()
                .Do(_ => _particle.Play(true));
        }

        IObservable<Unit> ITapEffect.OnHideAsObservable()
        {
            return Observable
                .ReturnUnit()
                .Do(_ => _particle.Stop());
        }
        #endregion
    }
}
