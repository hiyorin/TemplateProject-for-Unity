using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace SocialGame.Sound
{
    [Serializable]
    public sealed class SoundSettings
    {
        [SerializeField] private AudioMixerGroup _group = null;

        [SerializeField] private string _volumeExposedParameter = null;

        [SerializeField] [Range(0.0f, 1.0f)] private float _defaultVolume = 1.0f;

        public AudioMixerGroup Group { get { return _group; } }

        public string VolumeExposedParameter { get { return _volumeExposedParameter; } }

        public float DefaultVolume { get { return _defaultVolume; } }
    }

    [Serializable]
    public sealed class BGMSettings
    {
        [SerializeField] private AudioMixerGroup _group = null;

        [SerializeField] private string _volumeExposedParameter = null;

        [SerializeField] [Range(0.0f, 1.0f)] private float _defaultVolume = 1.0f;

        [SerializeField] [Range(0.0f, 1.0f)] private float _fadeInDuration = 0.2f;

        [SerializeField] [Range(0.0f, 1.0f)] private float _fadeOutDuration = 0.2f;

        [SerializeField] private List<AudioClip> _clips = null;
        
        public AudioMixerGroup Group { get { return _group; } }
        
        public string VolumeExposedParameter { get { return _volumeExposedParameter; } }

        public float DefaultVolume { get { return _defaultVolume; } }

        public float FadeInDuration { get { return _fadeInDuration; } }

        public float FadeOutDuration { get { return _fadeOutDuration; } }

        public IEnumerable<AudioClip> Clips { get { return _clips; } }
    }

    [Serializable]
    public sealed class SESettings
    {
        [SerializeField] private AudioMixerGroup _group = null;

        [SerializeField] private string _volumeExposedParameter = null;

        [SerializeField] [Range(0.0f, 1.0f)] private float _defaultVolume = 1.0f;

        [SerializeField] [Range(1, 100)] private int _maxPlayCount = 10;

        [SerializeField] private List<AudioClip> _clips = null;
        
        public AudioMixerGroup Group { get { return _group; } }
        
        public string VolumeExposedParameter { get { return _volumeExposedParameter; } }

        public float DefaultVolume { get { return _defaultVolume; } }

        public int MaxPlayCount { get { return _maxPlayCount; } }

        public IEnumerable<AudioClip> Clips { get { return _clips; } }
    }

    [Serializable]
    public sealed class VoiceSettings
    {
        [SerializeField] private AudioMixerGroup _group = null;

        [SerializeField] private string _volumeExposedParameter = null;

        [SerializeField] [Range(0.0f, 1.0f)] private float _defaultVolume = 1.0f;

        [SerializeField] [Range(1, 10)] private int _maxPlayCount = 10;

        [SerializeField] private List<AudioClip> _clips = null;
        
        public AudioMixerGroup Group { get { return _group; } }
        
        public string VolumeExposedParameter { get { return _volumeExposedParameter; } }

        public float DefaultVolume { get { return _defaultVolume; } }
        
        public int MaxPlayCount { get { return _maxPlayCount; } }

        public IEnumerable<AudioClip> Clips { get { return _clips; } }
    }

    [Serializable]
    public struct VolumeSettings
    {
        public float Master;
        public float BGM;
        public float SE;
        public float Voice;
    }
}
