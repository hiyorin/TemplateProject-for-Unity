using SocialGame.Loading;
using SocialGame.Internal.Loading.Builtin;
using UnityEngine;
using Zenject;

namespace SocialGame.Internal.Loading
{
    internal sealed class LoadingFactory : ILoadingFactory
    {
        [Inject] private DiContainer _container = null;

        [Inject] private LoadingSettings _settings = null;

        [Inject] private SampleLoading _samplePrefab = null;

        [Inject] private SystemLoading _systemLoading = null;
        
        #region ILoadingFactory implementation
        GameObject ILoadingFactory.Create(LoadingType type)
        {
            if (type == LoadingType.Sample)
                return _container.InstantiatePrefab(_samplePrefab);
            else if (type == LoadingType.System)
                return _container.InstantiatePrefab(_systemLoading);
            else
                return _container.InstantiatePrefab(_settings.Prefabs[(int)type]);
        }
        #endregion
    }
}
