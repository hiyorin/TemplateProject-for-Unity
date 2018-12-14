using System;
using SocialGame.Dialog;
using UnityEngine;
using Zenject;
using UniRx;

namespace Sandbox.Dialog
{
    public sealed class DialogTestModel : IInitializable, IDisposable
    {
        [Inject] private IDialogController _dialogController = null;

        private readonly CompositeDisposable _disposable = new CompositeDisposable();

        void IInitializable.Initialize()
        {
            Observable
                .EveryUpdate()
                .Where(_ => Input.GetKeyDown(KeyCode.O))
                .SelectMany(_ => _dialogController.Open<string>(DialogType.Sample, UnityEngine.Random.Range(0, 100).ToString()))
                .Subscribe(x => Debug.unityLogger.Log(GetType().Name, x))
                .AddTo(_disposable);
        }

        void IDisposable.Dispose()
        {
            _disposable.Dispose();
        }
    }
}
