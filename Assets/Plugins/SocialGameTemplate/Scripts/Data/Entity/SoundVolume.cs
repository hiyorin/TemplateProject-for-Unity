using System;

namespace SocialGame.Data.Entity
{
    [Serializable]
    public sealed class SoundVolume : ICloneable
    {
        public float Master;

        public float BGM;

        public float SE;

        public float Voice;
        
        public object Clone()
        {
            return MemberwiseClone();
        }

        public override string ToString()
        {
            return $"Master : {Master}\nBGM : {BGM}\nSE : {SE}\nVoice : {Voice}";
        }
    }
}