using System;
using System.Collections.Generic;
using SocialGame.Toast;
using SocialGame.Internal.Toast.Builtin;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityExtensions;
using Zenject;
using UniRx.Async;
using UnityObject = UnityEngine.Object;

namespace SocialGame.Internal.Toast
{
    internal sealed class ToastFactory : IInitializable, IDisposable, IToastFactory
    {
        [Inject] private DiContainer _container = null;

        [Inject] private ToastSettings _settings = null;

        [Inject] private SampleToast _samplePrefab = null;

        private readonly Dictionary<string, ObjectPool> _objectPools = new Dictionary<string, ObjectPool>();

        void IInitializable.Initialize()
        {
            foreach (ToastType type in Enum.GetValues(typeof(ToastType)))
            {
                UnityObject template;
                if (type == ToastType.Sample)
                    template = _samplePrefab;
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
        
        #region IToastFactory implementation
        async UniTask<GameObject> IToastFactory.Create(string name)
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
