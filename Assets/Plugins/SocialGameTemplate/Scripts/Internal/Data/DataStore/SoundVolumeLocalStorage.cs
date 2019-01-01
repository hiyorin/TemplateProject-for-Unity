using SocialGame.Data;
using SocialGame.Data.Entity;
using SocialGame.Internal.Sound;
using Zenject;

namespace SocialGame.Internal.Data.DataStore
{
    internal sealed class SoundVolumeLocalStorage : LocalStorageBase<SoundVolume>
    {
        [Inject] private SoundGeneralSettings _settings = null;
        
        protected override string FileName => "SoundVolume";

        protected override SoundVolume OnCreate()
        {
            return new SoundVolume()
            {
                Master  = _settings.DefaultMasterVolume,
                BGM     = _settings.DefaultBGMVolume,
                SE      = _settings.DefaultSEVolume,
                Voice   = _settings.DefaultVoiceVolume,
            };
        }
        
        protected override void OnInitialize()
        {

        }
        
#if UNITY_EDITOR
        [UnityEditor.CustomEditor(typeof(SoundVolumeLocalStorage))] 
        private class CustomInspector : CustomInspectorBase<SoundVolumeLocalStorage, SoundVolume>
        {
        }
#endif
    }
}