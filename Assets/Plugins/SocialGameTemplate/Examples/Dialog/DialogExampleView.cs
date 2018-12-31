using System;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using UniRx;

namespace Sandbox.Dialog
{
    internal sealed class DialogExampleView : MonoBehaviour
    {
        [Inject] private IDialogExampleModel _model = null;

        [SerializeField] private Text _message = null;

        private void Start()
        {
            _model.OnChangeMessageAsObservable()
                .Subscribe(x => _message.text = x)
                .AddTo(this);
        }
    }
}
