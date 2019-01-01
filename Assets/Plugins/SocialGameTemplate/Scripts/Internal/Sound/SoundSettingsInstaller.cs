using UnityEngine;
using Zenject;

namespace SocialGame.Internal.Sound
{
    internal enum SoundEngine
    {
        Unity,
        ADX2,
    }
    
    [CreateAssetMenu(fileName = "SoundSettings", menuName = "Installers/SoundSettings")]
    public sealed class SoundSettingsInstaller : ScriptableObjectInstaller<SoundSettingsInstaller>
    {
        [SerializeField] internal SoundEngine Engine = SoundEngine.Unity;
        
        [SerializeField] internal SoundGeneralSettings General = null;

        [SerializeField] internal Unity.UnityMasterSettings UnityMaster = null;
        
        [SerializeField] internal Unity.UnityBGMSettings UnityBgm = null;
        
        [SerializeField] internal Unity.UnitySESettings UnitySe = null;

        [SerializeField] internal Unity.UnityVoiceSettings UnityVoice = null;

        [SerializeField] internal ADX2.ADX2MasterSettings Adx2Master;
        
        [SerializeField] internal ADX2.ADX2BGMSettings Adx2Bgm = null;

        [SerializeField] internal ADX2.ADX2SESettings Adx2Se = null;

        [SerializeField] internal ADX2.ADX2VoiceSettings Adx2Voice;
        
        public override void InstallBindings()
        {
            Container.BindInstance(Engine).AsSingle();
            Container.BindInstance(General).AsSingle();
            
            switch (Engine)
            {
                case SoundEngine.Unity:
                    Container.BindInstance(UnityMaster).AsSingle();
                    Container.BindInstance(UnityBgm).AsSingle();
                    Container.BindInstance(UnitySe).AsSingle();
                    Container.BindInstance(UnityVoice).AsSingle();
                    break;
                case SoundEngine.ADX2:
                    Container.BindInstance(Adx2Master).AsSingle();
                    Container.BindInstance(Adx2Bgm).AsSingle();
                    Container.BindInstance(Adx2Se).AsSingle();
                    Container.BindInstance(Adx2Voice).AsSingle();
                    break;
                default:
                    Debug.unityLogger.LogError(GetType().Name, $"Not supported {Engine}");
                    break;
            }
        }
    }
}
