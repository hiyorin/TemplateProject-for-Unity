using System;
using SocialGame.Data.Entity;
using UniRx;

namespace SocialGame.Sound
{
    public interface ISoundVolumeController
    {
        IObservable<SoundVolume> Get();

        IObservable<Unit> Put(SoundVolume value);
    }
}