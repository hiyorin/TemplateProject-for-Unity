using System;
using UnityEngine;
using UnityObject = UnityEngine.Object;

namespace SocialGame.Loading
{
    [Serializable]
    public sealed class LoadingSettings
    {
        [SerializeField] private UnityObject[] _prefabs = null;
        [SerializeField] private float _defaultDuration = 0.5f;

        public UnityObject [] Prefabs { get { return _prefabs; } }
        public float DefaoutDuration { get { return _defaultDuration; } }
    }
}
