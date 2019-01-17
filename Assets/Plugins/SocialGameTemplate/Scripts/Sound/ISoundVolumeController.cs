using System;
using SocialGame.Data.Entity;
using UniRx;

namespace SocialGame.Sound
{
    public interface ISoundVolumeController
    {
        SoundVolume Get();

        void Put(SoundVolume value);

        IObservable<Unit> Save();
    }
}