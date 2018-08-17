using System;
using UnityEngine;
using UnityObject = UnityEngine.Object;

namespace SocialGame.Dialog
{
    [Serializable]
    public sealed class DialogSettings
    {
        [SerializeField] private UnityObject[] _prefabs = null;
        [SerializeField] private Color _maskColor = Color.white;
        [SerializeField] private float _defaultDuration = 0.5f;

        public UnityObject[] Prefabs { get { return _prefabs; } }
        public Color MaskColor { get { return _maskColor; } }
        public float DefaoutDuration { get { return _defaultDuration; } }
    }
}
