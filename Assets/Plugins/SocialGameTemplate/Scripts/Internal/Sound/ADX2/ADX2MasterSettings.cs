using System;
using UnityEngine;

namespace SocialGame.Internal.Sound.ADX2
{
    [Serializable]
    internal sealed class ADX2MasterSettings
    {
#if SGT_ADX2
        [SerializeField] private CriWareInitializer _initializerPrefab = null;

        public CriWareInitializer InitializerPrefab => _initializerPrefab;
#endif
    }

    [Serializable]
    internal sealed class ADX2BGMSettings
    {
#if SGT_ADX2
        [SerializeField] private string _categoryVolumeName = "BGM";
        
        [SerializeField] private CriAtomCueSheet _builtInCueSheet = null;

        public string CategoryVolumeName => _categoryVolumeName;
        
        public CriAtomCueSheet BuiltInCueSheet => _builtInCueSheet;
#endif
    }

    [Serializable]
    internal sealed class ADX2SESettings
    {
#if SGT_ADX2
        [SerializeField] private string _categoryVolumeName = "SE";
        
        [SerializeField] private CriAtomCueSheet _builtInCueSheet = null;

        public string CategoryVolumeName => _categoryVolumeName;
        
        public CriAtomCueSheet BuiltInCueSheet => _builtInCueSheet;
#endif
    }

    [Serializable]
    internal sealed class ADX2VoiceSettings
    {
#if SGT_ADX2
        [SerializeField] private string _categoryVolumeName = "Voice";
        
        [SerializeField] private CriAtomCueSheet _builtInCueSheet = null;

        public string CategoryVolumeName => _categoryVolumeName;
        
        public CriAtomCueSheet BuiltInCueSheet => _builtInCueSheet;
#endif
    }
}
