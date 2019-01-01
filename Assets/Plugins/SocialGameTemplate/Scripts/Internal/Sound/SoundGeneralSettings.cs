using System;
using UnityEngine;

namespace SocialGame.Internal.Sound
{
    [Serializable]
    internal sealed class SoundGeneralSettings
    {
        [SerializeField] [Range(0.0f, 1.0f)] private float _defaultMasterVolume = 1.0f;
        
        [SerializeField] [Range(0.0f, 1.0f)] private float _defaultBGMVolume = 1.0f;
        
        [SerializeField] [Range(0.0f, 1.0f)] private float _defaultSEVolume = 1.0f;
        
        [SerializeField] [Range(0.0f, 1.0f)] private float _defaultVoiceVolume = 1.0f;
        
        public float DefaultMasterVolume => _defaultMasterVolume;

        public float DefaultBGMVolume => _defaultBGMVolume;

        public float DefaultSEVolume => _defaultSEVolume;

        public float DefaultVoiceVolume => _defaultVoiceVolume;
    }
}