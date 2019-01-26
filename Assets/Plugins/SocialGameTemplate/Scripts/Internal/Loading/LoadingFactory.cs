using System;
using System.Collections.Generic;
using SocialGame.Loading;
using SocialGame.Internal.Loading.Builtin;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityExtensions;
using Zenject;
using UniRx.Async;
using UnityObject = UnityEngine.Object;

namespace SocialGame.Internal.Loading
{
    internal sealed class LoadingFactory : IInitializable, IDisposable, ILoadingFactory
    {
        [Inject] private DiContainer _container = null;

        [Inject] private LoadingSettings _settings = null;

        [Inject] private SampleLoading _samplePrefab = null;

        [Inject] private SystemLoading _systemLoading = null;

        private readonly Dictionary<string, ObjectPool> _objectPools = new Dictionary<string, ObjectPool>();

        void IInitializable.Initialize()
        {
            foreach (LoadingType type in Enum.GetValues(typeof(LoadingType)))
            {
                UnityObject template;
                if (type == LoadingType.Sample)
                    template = _samplePrefab;
                else if (type == LoadingType.System)
                    template = _systemLoading;
                else
                    template = _settings.Prefabs[(int)type];
                
                var pool = new ObjectPool(_container, template);
                _objectPools.Add(type.ToString(), pool);
            }
        }

        void IDisposable.Dispose()
        {
            _objectPools.Values.ForEach(x => x.Dispose());
            _objectPools.Clear();
        }
        
        #region ILoadingFactory implementation
        async UniTask<GameObject> ILoadingFactory.Create(string name)
        {
            ObjectPool pool;
            if (!_objectPools.TryGetValue(name, out pool))
            {
                var operation = Addressables.LoadAsset<GameObject>(name);
                await operation.ToUniTask();
                
                pool = new ObjectPool(_container, operation.Result);
                _objectPools.Add(name, pool);
            }
            
            return pool.Spawn();
        }
        #endregion
    }
}
