using System;
using UnityEngine;
using UnityObject = UnityEngine.Object;

namespace SocialGame.Internal.Toast
{
    [Serializable]
    internal sealed class ToastSettings
    {
        [SerializeField] private UnityObject[] _prefabs = null;

        [SerializeField] private Color _maskColor = new Color(0.0f, 0.0f, 0.0f, 0.75f);
        
        [SerializeField] private float _defaultDuration = 0.2f;
        
        [SerializeField] private float _showDuration = 2.0f;

        public UnityObject [] Prefabs => _prefabs;
        
        public Color MaskColor => _maskColor;
        
        public float DefaultDuration => _defaultDuration;
        
        public float ShowDuration => _showDuration;
    }
}
