using SocialGame.Loading;
using UnityEngine;
using Zenject;

namespace SocialGame.Internal.Loading
{
    internal sealed class LoadingFactory : ILoadingFactory
    {
        [Inject] private DiContainer _container = null;

        [Inject] private LoadingSettings _settings = null;

        [Inject] private SampleLoading _samplePrefab = null;

        #region ILoadingFactory implementation
        GameObject ILoadingFactory.Create(LoadingType type)
        {
            if (type == LoadingType.Sample)
                return _container.InstantiatePrefab(_samplePrefab);
            else
                return _container.InstantiatePrefab(_settings.Prefabs[(int)type]);
        }
        #endregion
    }
}
