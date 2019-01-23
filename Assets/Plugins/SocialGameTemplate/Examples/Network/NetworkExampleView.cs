using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Sandbox.Network
{
    internal sealed class NetworkExampleView : MonoBehaviour
    {
        [SerializeField] private Text _message = null;
        
        [Inject] private INetworkExampleModel _model = null;

        private void Start()
        {
            _model.OnChangeMessageAsObservable()
                .Subscribe(x => _message.text = x)
                .AddTo(this);
        }
    }
}