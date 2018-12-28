using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace SocialGame.Sound
{
    [Serializable]
    public struct VolumeSettings
    {
        public float Master;
        public float BGM;
        public float SE;
        public float Voice;
    }
}
