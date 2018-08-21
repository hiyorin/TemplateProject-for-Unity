using Zenject;

namespace SocialGame.Internal.Sound
{
    public sealed class SoundInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<SoundModel>().AsSingle();
            Container.BindInterfacesAndSelfTo<BGMModel>().AsSingle();
            Container.BindInterfacesAndSelfTo<SEModel>().AsSingle();
            Container.BindInterfacesAndSelfTo<VoiceModel>().AsSingle();
        }
    }
}
