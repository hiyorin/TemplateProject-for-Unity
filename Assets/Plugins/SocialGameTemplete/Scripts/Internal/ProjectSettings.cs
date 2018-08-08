using System;
using UnityEngine;

namespace SocialGame.Internal
{
    [Serializable]
    public sealed class DebugSettings
    {
        [SerializeField] private bool _fps = false;

        [SerializeField] private float _fpsUpdateIntervakl = 0.5f;

        public bool FPS { get { return _fps; } }

        public float FPSUpdateInterval { get { return _fpsUpdateIntervakl; } }
    }

    [Serializable]
    public sealed class ProjectSettings : ScriptableObject
    {
        [SerializeField] private DebugSettings _debug = null;

        public DebugSettings Debug { get { return _debug; } }
    }
}
