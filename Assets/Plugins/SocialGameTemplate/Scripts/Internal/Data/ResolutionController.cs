using SocialGame.Data;
using SocialGame.Internal.Data.DataStore;
using Zenject;
using UniRx;
using UniRx.Async;
using UnityEngine;

namespace SocialGame.Internal.Data
{
    internal sealed class ResolutionController : IInitializable, IResolutionController
    {
        [Inject] private IResolutionDataStore _datastore = null;

        [Inject] private ResolutionSettings _settings = null;

        private Vector2Int _defaultSize;
        
        private readonly CompositeDisposable _disposable = new CompositeDisposable();
        
        void IInitializable.Initialize()
        {
            _defaultSize = new Vector2Int(Screen.width, Screen.height);
            
            _datastore.Get()
                .ToObservable()
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
                    Debug.unityLogger.LogWarning(GetType().Name, $"Not supported {_settings.Type}");
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
                case Quality.Middle:
                    return _settings.MiddleSize;
                case Quality.High:
                    return _settings.HighSize;
                default:
                    Debug.unityLogger.LogWarning(GetType().Name, $"Not supported {quality}");
                    return _settings.MiddleSize;
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
                case Quality.Middle:
                    rate = _settings.MiddleRate;
                    break;
                case Quality.High:
                    rate = _settings.HighRate;
                    break;
                default:
                    Debug.unityLogger.LogWarning(GetType().Name, $"Not supported {quality}");
                    rate = _settings.MiddleRate;
                    break;
            }
            return new Vector2Int((int)(_defaultSize.x * rate), (int)(_defaultSize.y * rate));
        }

        #region IResolutionController implementation
        async UniTask<Quality> IResolutionController.Put(Quality quality)
        {
            Apply(quality);
            await _datastore.Put(quality);
            return quality;
        }

        async UniTask<Quality> IResolutionController.Get()
        {
            var result = await _datastore.Get();
            return result.Quality;
        }
        #endregion
    }
}
