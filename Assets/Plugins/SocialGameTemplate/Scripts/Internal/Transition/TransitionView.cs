using UnityEngine;
using Zenject;
using UniRx;

namespace SocialGame.Internal.Transition
{
    internal sealed class TransitionView : MonoBehaviour
    {
        [SerializeField] private Transform _container = null;

        [Inject] private ITransitionModel _model = null;

        private void Start()
        {
            _model.OnAddAsObservable()
                .Subscribe(x => x.transform.SetParent(_container, false))
                .AddTo(this);
        }
    }
}
