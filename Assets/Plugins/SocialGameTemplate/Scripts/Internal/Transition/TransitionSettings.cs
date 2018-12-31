using System;
using UnityEngine;
using UnityObject = UnityEngine.Object;

namespace SocialGame.Internal.Transition
{
    [Serializable]
    internal sealed class TransitionSettings
    {
        [SerializeField] private UnityObject[] _prefabs = null;

        [SerializeField] private float _defaultDuration = 0.2f;

        public UnityObject[] Prefabs => _prefabs;
        
        public float DefaultDuration => _defaultDuration;
    }
}
