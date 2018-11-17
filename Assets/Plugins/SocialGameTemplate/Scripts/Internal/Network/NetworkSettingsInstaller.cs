using UnityEngine;
using Zenject;
using SocialGame.Network;

namespace SocialGame.Internal.Network
{   
    [CreateAssetMenu(fileName = "NetworkSettings", menuName = "Installers/NetworkSettings")]
    public class NetworkSettingsInstaller : ScriptableObjectInstaller<NetworkSettingsInstaller>
    {
        [SerializeField] private GeneralSettings _generalSettings;
        
        [SerializeField] private HttpSettings _httpSettings = null;
        
        public override void InstallBindings()
        {
            Container.BindInstance(_generalSettings).AsSingle();
            Container.BindInstance(_httpSettings).AsSingle();
        }
    }
}
