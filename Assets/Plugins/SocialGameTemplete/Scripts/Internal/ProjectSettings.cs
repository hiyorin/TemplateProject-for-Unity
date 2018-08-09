using System;
using UnityEngine;

namespace SocialGame.Internal
{
    [Serializable]
    public sealed class ApplicationSettings
    {
        [SerializeField][Range(24, 60)] private int _targetFrameRate = 60;
        
        public int TargetFrameRate { get { return _targetFrameRate; } }
    }

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
        [SerializeField] private ApplicationSettings _application = null;

        [SerializeField] private DebugSettings _debug = null;

        public DebugSettings Debug { get { return _debug; } }

        public ApplicationSettings Application { get { return _application; } }
    }
}
