using System;
using System.Collections.Generic;
using SocialGame.Dialog;
using UnityEngine;
using UnityExtensions;
using Zenject;
using UnityObject = UnityEngine.Object;

namespace SocialGame.Internal.Dialog
{
    internal sealed class DialogFactory : IDisposable, IDialogFactory
    {
        [Inject] private DiContainer _container = null;

        [Inject] private DialogSettings _settings = null;

        [Inject] private SampleDialog _samplePrefab = null;

        private readonly Dictionary<int, ObjectPool> _objectPools = new Dictionary<int, ObjectPool>();

        void IDisposable.Dispose()
        {
            _objectPools.Values.ForEach(x => x.Dispose());
            _objectPools.Clear();
        }
        
        #region IDialogFactory implementation
        GameObject IDialogFactory.Spawn(DialogType type)
        {
            ObjectPool pool;
            if (!_objectPools.TryGetValue((int) type, out pool))
            {
                UnityObject template;
                if (type == DialogType.Sample)
                    template = _samplePrefab;
                else
                    template = _settings.Prefabs[(int)type];
                
                pool = new ObjectPool(_container, template);
                _objectPools.Add((int)type, pool);
            }
            
            return pool.Spawn();
        }

        void IDialogFactory.Despawn(DialogType type, GameObject value)
        {
            ObjectPool pool;
            if (!_objectPools.TryGetValue((int) type, out pool))
                return;
            
            pool.Despawn(value);
        }
        #endregion
    }
}
