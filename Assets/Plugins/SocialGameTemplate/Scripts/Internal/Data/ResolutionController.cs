using System;
using SocialGame.Data;
using SocialGame.Internal.Data.DataStore;
using SocialGame.Internal.Data.Entity;
using Zenject;
using UniRx;
using UnityEngine;

namespace SocialGame.Internal.Data
{
    internal sealed class ResolutionController : IInitializable, IResolutionController
    {
        [Inject] private IResolutionDataStore _datastore;

        [Inject] private ResolutionSettings _settings;

        private Vector2Int _defaultSize;
        
        private readonly CompositeDisposable _disposable = new CompositeDisposable();
        
        void IInitializable.Initialize()
        {
            _defaultSize = new Vector2Int(Screen.width, Screen.height);
            
            _datastore
                .Get()
                .Subscribe(x => Apply(x.Quality))
                .AddTo(_disposable);
        }
        
        private void Apply(Quality quality)
        {
            Vector2Int resolution;
            switch (_settings.Type)
            {
                case ResolutionSettings.ResType.Static:
                    resolution = GetResolutionSize(quality);
                    break;
                case ResolutionSettings.ResType.Variable:
                    resolution = GetResolutionRate(quality);
                    break;
                default:
                    Debug.unityLogger.LogWarning(GetType().Name, string.Format("Not supported {0}", _settings.Type));
                    resolution = _defaultSize;
                    break;
            }
            Screen.SetResolution(resolution.x, resolution.y, true);
        }

        private Vector2Int GetResolutionSize(Quality quality)
        {
            switch (quality)
            {
                case Quality.Low:
                    return _settings.LowSize;
                case Quality.Standard:
                    return _settings.StandardSize;
                case Quality.High:
                    return _settings.HighSize;
                default:
                    Debug.unityLogger.LogWarning(GetType().Name, string.Format("Not supported {0}", quality));
                    return _settings.StandardSize;
            }
        }

        private Vector2Int GetResolutionRate(Quality quality)
        {
            float rate;
            switch (quality)
            {
                case Quality.Low:
                    rate = _settings.LowRate;
                    break;
                case Quality.Standard:
                    rate = _settings.StandardRate;
                    break;
                case Quality.High:
                    rate = _settings.HighRate;
                    break;
                default:
                    Debug.unityLogger.LogWarning(GetType().Name, string.Format("Not supported {0}", quality));
                    rate = _settings.StandardRate;
                    break;
            }
            return new Vector2Int((int)(_defaultSize.x * rate), (int)(_defaultSize.y * rate));
        }

        #region IResolutionController implementation
        IObservable<Unit> IResolutionController.Put(Quality quality)
        {
            GetResolutionSize(quality);
            return _datastore.Put(quality);
        }

        IObservable<Quality> IResolutionController.Get()
        {
            return _datastore.Get().Select(x => x.Quality);
        }
        #endregion
    }
}
