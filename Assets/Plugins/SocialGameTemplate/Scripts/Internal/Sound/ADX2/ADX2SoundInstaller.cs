#if SGT_ADX2
using Zenject;

namespace SocialGame.Internal.Sound.ADX2
{
    internal sealed class ADX2SoundInstaller : Installer
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesTo<ADX2MasterModel>().AsSingle();
            Container.BindInterfacesTo<ADX2BGMModel>().AsSingle();
            Container.BindInterfacesTo<ADX2SEModel>().AsSingle();
            Container.BindInterfacesTo<ADX2VoiceModel>().AsSingle();
        }
    }
}
#endif
