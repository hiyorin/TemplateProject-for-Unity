using System;
using UnityEngine;

namespace SocialGame.Internal
{
    [Serializable]
    public sealed class ApplicationSettings
    {
        [SerializeField][Range(24, 60)] private int _targetFrameRate = 60;

        [SerializeField][Range(0.1f, 1.0f)] private float _resolution = 1.0f;

        public int TargetFrameRate { get { return _targetFrameRate; } }

        public float Resolution { get { return _resolution; } }
    }

    [Serializable]
    public sealed class DebugSettings
    {
        [Header("General")]
        [SerializeField] private Color _textColor = Color.white;

        [SerializeField] private int _textFontSize = 24;

        [Header("FPS")]
        [SerializeField] private bool _fps = false;

        [SerializeField] private float _fpsUpdateIntervakl = 0.5f;

        [Header("Memory")]
        [SerializeField] private bool _memory = false;

        [SerializeField] private float _memoryUpdateInterval = 0.5f;

        [Header("Extension")]
        [SerializeField] private bool _extension = false;

        [SerializeField] private float _extensionUpdateInterval = 0.5f;

        public Color TextColor { get { return _textColor; } }

        public int TextFontSize { get { return _textFontSize; } }

        public bool FPS { get { return _fps; } }

        public float FPSUpdateInterval { get { return _fpsUpdateIntervakl; } }

        public bool Memory { get { return _memory; } }

        public float MemoryUpdateInterval { get { return _memoryUpdateInterval; } }

        public bool Extension { get { return _extension; } }

        public float ExtensionUpdateInterval { get { return _extensionUpdateInterval; } }
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
