using System;
using UnityEngine;
using UnityObject = UnityEngine.Object;

namespace SocialGame.Internal.TapEffect
{
    [Serializable]
    internal sealed class TapEffectSettings
    {
        [SerializeField] private UnityObject[] _prefabs = null;

        public UnityObject [] Prefabs => _prefabs;
    }
}
