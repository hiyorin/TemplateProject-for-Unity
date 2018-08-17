using System;
using UnityEngine;
using UnityObject = UnityEngine.Object;

namespace SocialGame.TapEffect
{
    [Serializable]
    public sealed class TapEffectSettings
    {
        [SerializeField] private UnityObject[] _prefabs = null;

        public UnityObject [] Prefabs { get { return _prefabs; } }
    }
}
