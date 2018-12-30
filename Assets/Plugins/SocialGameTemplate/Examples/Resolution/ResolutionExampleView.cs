using System;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using UniRx;

namespace Sandbox.Resolution
{
    internal sealed class ResolutionExampleView : MonoBehaviour
    {
        [SerializeField] private Text _message = null;

        [Inject] private IResolutionExampleModel _model = null;

        private void Start()
        {
            _model.OnChangeMessageAsObservable()
                .Subscribe(x => _message.text = x)
                .AddTo(this);
        }
    }
}
