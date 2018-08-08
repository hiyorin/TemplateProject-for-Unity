using System;
using UnityEngine;
using Zenject;
using UniRx;

namespace SocialGame.Internal.DebugMode
{
    internal interface IFPSModel
    {
        IObservable<float> OnUpdateFPSAsObservable();
    }

    internal sealed class FPSModel : IInitializable, IDisposable, IFPSModel
    {
        [Inject] private DebugSettings _settings = null;

        private readonly FloatReactiveProperty _fps = new FloatReactiveProperty();

        private readonly CompositeDisposable _disposable = new CompositeDisposable();

        private float _accum;
        private int _frames;
        private float _timeleft;

        void IInitializable.Initialize()
        {
            if (!_settings.FPS)
                return;

            Observable
                .EveryUpdate()
                .Subscribe(_ => {
                    _timeleft -= Time.deltaTime;
                    _accum += Time.timeScale / Time.deltaTime;
                    _frames++;

                    if (0 < _timeleft) return;

                    _fps.Value = _accum / _frames;
                    _timeleft = _settings.FPSUpdateInterval;
                    _accum = 0;
                    _frames = 0;
                })
                .AddTo(_disposable);
        }

        void IDisposable.Dispose()
        {
            _disposable.Dispose();
        }

        #region IDialogModel implementation
        IObservable<float> IFPSModel.OnUpdateFPSAsObservable()
        {
            return _fps.Skip(1);
        }
        #endregion
    }
}
