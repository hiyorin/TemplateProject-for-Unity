using Zenject;

namespace SocialGame.Internal.Sound
{
    internal sealed class SoundInstaller : MonoInstaller
    {
        [Inject] private SoundEngine _soundEngine = SoundEngine.Unity;
        
        public override void InstallBindings()
        {
            if (_soundEngine == SoundEngine.Unity)
                Container.Install<Unity.UnitySoundInstaller>();
            else
                Container.Install<ADX2.ADX2SoundInstaller>();
        }
    }
}
