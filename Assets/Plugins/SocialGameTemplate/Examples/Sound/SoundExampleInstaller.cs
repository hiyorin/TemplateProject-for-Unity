using UnityEngine;
using Zenject;

namespace SocialGame.Examples.Sound
{
    internal sealed class SoundExampleInstaller : MonoInstaller<SoundExampleInstaller>
    {
        [SerializeField] private SoundExampleIntent _intent = null;
        
        public override void InstallBindings()
        {
            Container.BindInterfacesTo<SoundExampleModel>().AsSingle();
            Container.BindInstance<ISoundExampleIntent>(_intent).AsSingle();
        }
    }
}