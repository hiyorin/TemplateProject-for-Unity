using System;
using System.Collections.Generic;
using SocialGame.Internal.Transition.Builtin;
using SocialGame.Transition;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Zenject;
using UniRx.Async;
using UnityExtensions;
using UnityObject = UnityEngine.Object;

namespace SocialGame.Internal.Transition
{
    internal sealed class TransitionFactory : IInitializable, IDisposable, ITransitionFactory
    {
        [Inject] private DiContainer _container = null;

        [Inject] private TransitionSettings _settings = null;

        [Inject] private BlackFadeTransition _blackFadePrefab = null;

        private readonly Dictionary<string, ObjectPool> _objectPools = new Dictionary<string, ObjectPool>();

        void IInitializable.Initialize()
        {
            foreach (TransMode type in Enum.GetValues(typeof(TransMode)))
            {
                UnityObject template;
                if (type == TransMode.BlackFade)
                    template = _blackFadePrefab;
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
        
        #region ITransitionFactory implementation
        async UniTask<GameObject> ITransitionFactory.Create(string name)
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
