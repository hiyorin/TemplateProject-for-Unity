using Zenject;

namespace SocialGame.Internal.Sound.Unity
{
    internal sealed class UnitySoundInstaller : Installer
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesTo<UnityMasterModel>().AsSingle();
            Container.BindInterfacesTo<UnityBGMModel>().AsSingle();
            Container.BindInterfacesTo<UnitySEModel>().AsSingle();
            Container.BindInterfacesTo<UnityVoiceModel>().AsSingle();
        }
    }
}