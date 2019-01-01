using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace SocialGame.Internal.Sound.Unity
{
    [Serializable]
    internal sealed class UnityMasterSettings
    {
        [SerializeField] private AudioMixerGroup _group = null;

        [SerializeField] private string _volumeExposedParameter = "MasterVolume";
        
        public AudioMixerGroup Group => _group;

        public string VolumeExposedParameter => _volumeExposedParameter;
    }
    
    [Serializable]
    public sealed class UnityBGMSettings
    {
        [SerializeField] private AudioMixerGroup _group = null;

        [SerializeField] private string _volumeExposedParameter = "MasterVolume";

        [SerializeField] [Range(0.0f, 1.0f)] private float _fadeInDuration = 0.2f;

        [SerializeField] [Range(0.0f, 1.0f)] private float _fadeOutDuration = 0.2f;

        [SerializeField] private List<AudioClip> _clips = null;
        
        public AudioMixerGroup Group => _group;
        
        public string VolumeExposedParameter => _volumeExposedParameter;

        public float FadeInDuration => _fadeInDuration;

        public float FadeOutDuration =>_fadeOutDuration;

        public IEnumerable<AudioClip> Clips => _clips;
    }
    
    [Serializable]
    public sealed class UnitySESettings
    {
        [SerializeField] private AudioMixerGroup _group = null;

        [SerializeField] private string _volumeExposedParameter = "SEVolume";

        [SerializeField] [Range(1, 100)] private int _maxPlayCount = 10;

        [SerializeField] private List<AudioClip> _clips = null;
        
        public AudioMixerGroup Group => _group;
        
        public string VolumeExposedParameter => _volumeExposedParameter;

        public int MaxPlayCount => _maxPlayCount;

        public IEnumerable<AudioClip> Clips => _clips;
    }
    
    [Serializable]
    public sealed class UnityVoiceSettings
    {
        [SerializeField] private AudioMixerGroup _group = null;

        [SerializeField] private string _volumeExposedParameter = "VoiceVolume";

        [SerializeField] [Range(1, 10)] private int _maxPlayCount = 10;

        [SerializeField] private List<AudioClip> _clips = null;
        
        public AudioMixerGroup Group => _group;
        
        public string VolumeExposedParameter => _volumeExposedParameter;

        public int MaxPlayCount => _maxPlayCount;

        public IEnumerable<AudioClip> Clips => _clips;
    }
}