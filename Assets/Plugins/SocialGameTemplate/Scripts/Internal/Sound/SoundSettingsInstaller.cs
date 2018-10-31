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
    [CreateAssetMenu(fileName = "SoundSettings", menuName = "Installers/SoundSettings")]
    public sealed class SoundSettingsInstaller : ScriptableObjectInstaller<SoundSettingsInstaller>
    {
        [SerializeField] private SoundSettings _general = null;

        [SerializeField] private BGMSettings _bgm = null;

        [SerializeField] private SESettings _se = null;

        [SerializeField] private VoiceSettings _voice = null;

        public override void InstallBindings()
        {
            Container.BindInstance(_general).AsSingle();
            Container.BindInstance(_bgm).AsSingle();
            Container.BindInstance(_se).AsSingle();
            Container.BindInstance(_voice).AsSingle();
        }

        #if UNITY_EDITOR
        public void OnValidate()
        {
            if (EditorApplication.isPlaying)
                return;

            bool refresh = false;
            refresh |= Generate("BGM", _bgm.Clips);
            refresh |= Generate("SE", _se.Clips);
            refresh |= Generate("Voice", _voice.Clips);
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
