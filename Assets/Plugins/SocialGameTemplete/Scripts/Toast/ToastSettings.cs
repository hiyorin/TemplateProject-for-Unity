using System;
using UnityEngine;
using UnityObject = UnityEngine.Object;

namespace SocialGame.Toast
{
    [Serializable]
    public sealed class ToastSettings
    {
        [SerializeField] private UnityObject[] _prefabs = null;
        [SerializeField] private Color _maskColor = Color.white;
        [SerializeField] private float _defaoutDuration = 0.5f;
        [SerializeField] private float _showDuration = 2.0f;

        public UnityObject [] Prefabs { get { return _prefabs; } }
        public Color MaskColor { get { return _maskColor; } }
        public float DefaoutDuration { get { return _defaoutDuration; } }
        public float ShowDuration { get { return _showDuration; } }
    }
}
