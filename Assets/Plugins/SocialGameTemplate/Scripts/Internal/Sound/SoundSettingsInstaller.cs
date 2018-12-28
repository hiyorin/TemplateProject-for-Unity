using System;
using System.Linq;
using System.Collections.Generic;
using SocialGame.Sound;
using UnityEngine;
using UnityExtensions;
using Zenject;
#if UNITY_EDITOR
using System.IO;
using System.Text;
using UnityEditor;
using UnityEditor.VersionControl;
#endif

namespace SocialGame.Internal.Sound
{
    internal enum Type
    {
        Unity,
        ADX2,
    }
    
    [CreateAssetMenu(fileName = "SoundSettings", menuName = "Installers/SoundSettings")]
    public sealed class SoundSettingsInstaller : ScriptableObjectInstaller<SoundSettingsInstaller>
    {
        [SerializeField] internal Type Type = Type.Unity;
        
        [SerializeField] internal GeneralSettings General = null;

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
            Container.BindInstance(General).AsSingle();
            
            switch (Type)
            {
                case Type.Unity:
                    Container.BindInstance(UnityMaster).AsSingle();
                    Container.BindInstance(UnityBgm).AsSingle();
                    Container.BindInstance(UnitySe).AsSingle();
                    Container.BindInstance(UnityVoice).AsSingle();
                    break;
                case Type.ADX2:
                    Container.BindInstance(Adx2Master).AsSingle();
                    Container.BindInstance(Adx2Bgm).AsSingle();
                    Container.BindInstance(Adx2Se).AsSingle();
                    Container.BindInstance(Adx2Voice).AsSingle();
                    break;
                default:
                    Debug.unityLogger.LogError(GetType().Name, $"Not supported {Type}");
                    break;
            }
        }

        #if UNITY_EDITOR
        public void OnValidate()
        {
            if (EditorApplication.isPlaying)
                return;

            bool refresh = false;
            refresh |= Generate(typeof(BGM).Name, UnityBgm.Clips);
            refresh |= Generate(typeof(SE).Name, UnitySe.Clips);
            refresh |= Generate(typeof(Voice).Name, UnityVoice.Clips);
            if (!refresh)
                return;
            
            AssetDatabase.Refresh(ImportAssetOptions.ImportRecursive);

            Debug.unityLogger.Log(GetType().Name, "auto-generated Sound");
        }
        
        private bool Generate(string type, IEnumerable<AudioClip> clips)
        {
            string fileName = Path.Combine(ProjectModel.RootPath, "Scripts/Sound", type + ".cs");
            string filePath = Path.Combine(Application.dataPath, fileName);
            
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("// this file was auto-generated.");
            builder.AppendLine("namespace SocialGame.Sound");
            builder.AppendLine("{");
            builder.AppendLine("    public enum " + type);
            builder.AppendLine("    {");

            foreach (var clip in clips.Where(x => x != null))
            {
                builder.AppendLineFormat("        {0},", clip.name);
            }
            
            builder.AppendLine("    }");
            builder.AppendLine("}");            
            
            string text = builder.ToString();
            if (File.Exists(filePath))
            {
                if (File.ReadAllText(filePath) == text)
                    return false;
                
                if (Provider.isActive)
                    Provider.Checkout(Path.Combine("Assets", fileName), CheckoutMode.Asset).Wait();
            }
            
            File.WriteAllText(filePath, text, Encoding.UTF8);
            return true;
        }
        #endif
    }
}
