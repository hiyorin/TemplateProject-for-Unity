using Zenject;

namespace SocialGame.Internal.Sound
{
    internal sealed class SoundInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
#if SGT_ADX2
            Container.Install<ADX2.ADX2SoundInstaller>();
#else
            Container.Install<Unity.UnitySoundInstaller>();
#endif
        }
    }
}
