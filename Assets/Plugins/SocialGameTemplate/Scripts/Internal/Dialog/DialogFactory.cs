using System;
using System.Collections.Generic;
using SocialGame.Dialog;
using SocialGame.Internal.Dialog.Builtin;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityExtensions;
using Zenject;
using UniRx.Async;
using UnityObject = UnityEngine.Object;

namespace SocialGame.Internal.Dialog
{
    internal sealed class DialogFactory : IInitializable, IDisposable, IDialogFactory
    {
        [Inject] private DiContainer _container = null;

        [Inject] private DialogSettings _settings = null;

        [Inject] private SampleDialog _samplePrefab = null;

        private readonly Dictionary<string, ObjectPool> _objectPools = new Dictionary<string, ObjectPool>();

        void IInitializable.Initialize()
        {
            foreach (DialogType type in Enum.GetValues(typeof(DialogType)))
            {
                UnityObject template;
                if (type == DialogType.Sample)
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
        
        #region IDialogFactory implementation
        async UniTask<GameObject> IDialogFactory.Spawn(string name)
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

        void IDialogFactory.Despawn(string name, GameObject value)
        {
            ObjectPool pool;
            if (!_objectPools.TryGetValue(name, out pool))
                return;
            
            pool.Despawn(value);
        }
        #endregion
    }
}
