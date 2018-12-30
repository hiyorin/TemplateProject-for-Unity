using System;
using UnityEngine;
using UnityObject = UnityEngine.Object;

namespace SocialGame.Internal.Loading
{
    [Serializable]
    internal sealed class LoadingSettings
    {
        [SerializeField] private UnityObject[] _prefabs = null;

        [SerializeField] private Color _maskColor = new Color(0.0f, 0.0f, 0.0f, 0.75f);
        
        [SerializeField] private float _defaultDuration = 0.2f;

        public UnityObject [] Prefabs => _prefabs;

        public Color MaskColor => _maskColor;
        
        public float DefaultDuration => _defaultDuration;
    }
}
