using System;
using System.Collections.Generic;
using SocialGame.Internal.TapEffect.Builtin;
using SocialGame.TapEffect;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Zenject;
using UniRx.Async;
using UnityExtensions;
using UnityObject = UnityEngine.Object;

namespace SocialGame.Internal.TapEffect
{
    internal sealed class TapEffectFactory : IInitializable, IDisposable, ITapEffectFactory
    {
        [Inject] private DiContainer _container = null;

        [Inject] private TapEffectSettings _settings = null;

        [Inject] private SampleTapEffect _samplePrefab = null;

        private readonly Dictionary<string, ObjectPool> _objectPools = new Dictionary<string, ObjectPool>();

        void IInitializable.Initialize()
        {
            foreach (TapEffectType type in Enum.GetValues(typeof(TapEffectType)))
            {
                UnityObject template;
                if (type == TapEffectType.Sample)
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
        
        #region ITapEffectFactory implementation
        async UniTask<GameObject> ITapEffectFactory.Create(string name)
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
