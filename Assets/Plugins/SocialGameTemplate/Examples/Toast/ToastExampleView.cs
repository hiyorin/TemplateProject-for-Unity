using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Sandbox.Toast
{
    internal sealed class ToastExampleView : MonoBehaviour
    {
        [SerializeField] private Text _message = null;
        
        [Inject] private IToastExampleModel _model = null;

        private void Start()
        {
            _model.OnChangeMessageAsObservable()
                .Subscribe(x => _message.text = x)
                .AddTo(this);
        }
    }
}