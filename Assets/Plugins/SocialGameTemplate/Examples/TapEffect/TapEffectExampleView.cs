using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Sandbox.TapEffect
{
    internal sealed class TapEffectExampleView : MonoBehaviour
    {
        [SerializeField] private Text _message = null;
        
        [Inject] private ITapEffectExampleModel _model = null;

        private void Start()
        {
            _model.OnChangeMessageAsObservable()
                .Subscribe(x => _message.text = x)
                .AddTo(this);
        }
    }
}