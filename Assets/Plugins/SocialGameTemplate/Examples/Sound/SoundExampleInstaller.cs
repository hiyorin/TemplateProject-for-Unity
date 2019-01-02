using UnityEngine;
using Zenject;

namespace SocialGame.Examples.Sound
{
    internal enum SoundType
    {
        BGM,
        SE,
        Voice,
    }
    
    internal sealed class SoundExampleInstaller : MonoInstaller<SoundExampleInstaller>
    {
        [SerializeField] private SoundExampleIntent _bgmIntent = null;

        [SerializeField] private SoundExampleIntent _seIntent = null;

        [SerializeField] private SoundExampleIntent _voiceIntent = null;
        
        public override void InstallBindings()
        {
            Container.BindInterfacesTo<SoundExampleModel>().AsSingle();
            Container.BindInstance<ISoundExampleIntent>(_bgmIntent).WithId(SoundType.BGM).AsTransient();
            Container.BindInstance<ISoundExampleIntent>(_seIntent).WithId(SoundType.SE).AsTransient();
            Container.BindInstance<ISoundExampleIntent>(_voiceIntent).WithId(SoundType.Voice).AsTransient();
        }
    }
}