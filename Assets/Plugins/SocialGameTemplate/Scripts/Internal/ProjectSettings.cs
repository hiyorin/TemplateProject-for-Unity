using System;
using UnityEngine;

namespace SocialGame.Internal
{
    [Serializable]
    internal sealed class ApplicationSettings
    {
        [SerializeField][Range(24, 60)] private int _targetFrameRate = 60;
        
        public int TargetFrameRate { get { return _targetFrameRate; } }
    }

    [Serializable]
    internal sealed class DebugSettings
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
        public const string FileName = "ProjectSettings";
        
        [SerializeField] private ApplicationSettings _application = null;

        [SerializeField] private DebugSettings _debug = null;

        internal DebugSettings Debug { get { return _debug; } }

        internal ApplicationSettings Application { get { return _application; } }
    }
}
