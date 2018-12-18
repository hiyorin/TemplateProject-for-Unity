using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using UnityObject = UnityEngine.Object;

namespace SocialGame.Internal
{
    internal sealed class ObjectPool : IDisposable
    {
        private readonly DiContainer _diContainer;
        
        private readonly UnityObject _template;
        
        private readonly List<GameObject> _actives = new List<GameObject>();
        
        private readonly Stack<GameObject> _deactives = new Stack<GameObject>();
        
        public ObjectPool(DiContainer diContainer, UnityObject template)
        {
            _diContainer = diContainer;
            _template = template;
        }

        public void Dispose()
        {
            DestroyContainer(_actives);
            DestroyContainer(_deactives);
            
            _actives.Clear();
            _deactives.Clear();
        }

        private void DestroyContainer(IEnumerable<GameObject> containers)
        {
            foreach (var obj in containers)
                UnityEngine.Object.DestroyImmediate(obj);
        }
        
        public GameObject Spawn()
        {
            GameObject result;
            if (_deactives.Count > 0)
                result = _deactives.Pop();
            else
                result = _diContainer.InstantiatePrefab(_template);
            
            _actives.Add(result);
            
            result.SetActive(true);
            return result;
        }

        public void Despawn(GameObject value)
        {
            if (!_actives.Contains(value))
                return;

            value.SetActive(false);
            _actives.Remove(value);
            _deactives.Push(value);
        }
    }
}