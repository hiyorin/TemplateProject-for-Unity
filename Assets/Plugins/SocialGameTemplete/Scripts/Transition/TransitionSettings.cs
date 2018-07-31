using System;
using UnityEngine;
using UnityObject = UnityEngine.Object;

namespace SocialGame.Transition
{
    [Serializable]
    public sealed class TransitionSettings
    {
        [SerializeField] private UnityObject[] _prefabs = null;
        [SerializeField] private float _defaultDuration = 0.5f;

        public UnityObject[] Prefabs { get { return _prefabs; } }
        public float DefaultDuration { get { return _defaultDuration; } }
    }
}
