using System;
using UnityEngine;
using UnityObject = UnityEngine.Object;

namespace SocialGame.Internal.Loading
{
    [Serializable]
    internal sealed class LoadingSettings
    {
        [SerializeField] private Color _maskColor = new Color(0.0f, 0.0f, 0.0f, 0.75f);
        
        [SerializeField] private float _defaultDuration = 0.2f;
        
        [SerializeField] private UnityObject[] _prefabs = null;
    
        public Color MaskColor => _maskColor;
        
        public float DefaultDuration => _defaultDuration;

        public UnityObject[] Prefabs => _prefabs;
    }
}
