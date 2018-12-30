using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Sandbox.Loading
{
    internal sealed class LoadingExampleView : MonoBehaviour
    {
        [SerializeField] private Text _message = null;
        
        [Inject] private ILoadingExampleModel _model;

        private void Start()
        {
            _model.OnChangeMessageAsObservable()
                .Subscribe(x => _message.text = x)
                .AddTo(this);
        }
    }
}