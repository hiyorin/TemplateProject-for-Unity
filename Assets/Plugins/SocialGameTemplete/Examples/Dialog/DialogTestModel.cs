using System;
using SocialGame.Dialog;
using UnityEngine;
using Zenject;
using UniRx;

namespace Sandbox.Dialog
{
    public class DialogTestModel : IInitializable, IDisposable
    {
        [Inject] private DialogController _dialogController = null;

        private readonly CompositeDisposable _disposable = new CompositeDisposable();

        void IInitializable.Initialize()
        {
            Observable
                .EveryUpdate()
                .Where(_ => Input.GetKeyDown(KeyCode.O))
                .SelectMany(_ => _dialogController.Open(DialogType.Sample, UnityEngine.Random.Range(0, 100).ToString()))
                .Subscribe(x => Debug.Log(x))
                .AddTo(_disposable);
        }

        void IDisposable.Dispose()
        {
            _disposable.Dispose();
        }
    }
}
