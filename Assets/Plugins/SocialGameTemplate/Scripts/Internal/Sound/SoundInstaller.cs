using Zenject;

namespace SocialGame.Internal.Sound
{
    public sealed class SoundInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesTo<SoundModel>().AsSingle();
            Container.BindInterfacesTo<BGMModel>().AsSingle();
            Container.BindInterfacesTo<SEModel>().AsSingle();
            Container.BindInterfacesTo<VoiceModel>().AsSingle();
        }
    }
}
