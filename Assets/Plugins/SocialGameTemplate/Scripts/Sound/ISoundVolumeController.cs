using SocialGame.Data.Entity;
using UniRx.Async;

namespace SocialGame.Sound
{
    public interface ISoundVolumeController
    {
        SoundVolume Get();

        void Put(SoundVolume value);

        UniTask Save();
    }
}