using SocialGame.TapEffect;
using UnityEngine;
using Zenject;
using UniRx.Async;

namespace SocialGame.Internal.TapEffect.Builtin
{
    internal sealed class SampleTapEffect : MonoBehaviour, ITapEffect
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
            Vector3 worldPosition;
            RectTransformUtility.ScreenPointToWorldPointInRectangle(_transform, position, _uiCamera, out worldPosition);
            _transform.position = worldPosition;
        }

        async UniTask ITapEffect.OnShow()
        {
            _particle.Play(true);
        }

        async UniTask ITapEffect.OnHide()
        {
            _particle.Stop();
        }
        #endregion
    }
}
