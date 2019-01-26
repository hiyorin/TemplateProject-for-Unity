using System;
using UnityEngine;
using UnityObject = UnityEngine.Object;

namespace SocialGame.Internal.Transition
{
    [Serializable]
    internal sealed class TransitionSettings
    {
        [SerializeField] private float _defaultDuration = 0.2f;

        [SerializeField] private UnityObject[] _prefabs = null;
        
        public float DefaultDuration => _defaultDuration;

        public UnityObject[] Prefabs => _prefabs;
    }
}
