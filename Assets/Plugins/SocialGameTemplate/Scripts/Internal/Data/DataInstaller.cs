using SocialGame.Internal.Data.DataStore;
using SocialGame.Internal.Sound;
using UnityEngine;
using Zenject;

namespace SocialGame.Internal.Data
{
    internal sealed class DataInstaller : MonoInstaller<DataInstaller>
    {
        [SerializeField] private ResolutionLocalStorage _resolutionStorage = null;

        [SerializeField] private SoundVolumeLocalStorage _soundVolumeStorage = null;
        
        public override void InstallBindings()
        {
            Container.BindInstance(_resolutionStorage).AsSingle();
            Container.BindInstance(_soundVolumeStorage).AsSingle();
            
            Container.BindInterfacesTo<ResolutionLocalDataStore>().AsSingle();
            Container.BindInterfacesTo<ResolutionController>().AsSingle();
            Container.BindInterfacesTo<SoundVolumeController>().AsSingle();
        }
    }
}