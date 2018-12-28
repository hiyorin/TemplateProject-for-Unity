using System;
using UnityEngine;

namespace SocialGame.Internal.Sound.ADX2
{
    [Serializable]
    internal sealed class ADX2MasterSettings
    {
        [SerializeField] private CriWareInitializer _initializerPrefab;

        public CriWareInitializer InitializerPrefab => _initializerPrefab;
    }

    [Serializable]
    internal sealed class ADX2BGMSettings
    {
    }

    [Serializable]
    internal sealed class ADX2SESettings
    {
    }

    [Serializable]
    internal sealed class ADX2VoiceSettings
    {
    }
}