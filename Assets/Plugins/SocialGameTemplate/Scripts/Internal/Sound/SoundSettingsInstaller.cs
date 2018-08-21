using System.Linq;
using System.Collections.Generic;
using SocialGame.Sound;
using UnityEngine;
using Zenject;
#if UNITY_EDITOR
using System.IO;
using System.Text;
using UnityEditor;
#endif

namespace SocialGame.Internal.Sound
{
    [CreateAssetMenu(fileName = "SoundSettings", menuName = "Installers/SoundSettingsInstaller")]
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
            Generate("BGM", _bgm.Clips);
            Generate("SE", _se.Clips);
            Generate("Voice", _voice.Clips);

            AssetDatabase.Refresh(ImportAssetOptions.ImportRecursive);

            Debug.unityLogger.Log(GetType().Name, "auto-generated Sound");
        }

        private void Generate(string type, IEnumerable<AudioClip> clips)
        {
            string path = Path.Combine(Application.dataPath, ProjectModel.RootPath, "Scripts/Sound", type + ".cs");
            using (StreamWriter writer = new StreamWriter(path, false, Encoding.UTF8))
            {
                writer.WriteLine("// this file was auto-generated.");
                writer.WriteLine("namespace SocialGame.Sound");
                writer.WriteLine("{");
                writer.WriteLine("    public enum " + type);
                writer.WriteLine("    {");

                foreach (var clip in clips.Where(x => x != null))
                {
                    writer.WriteLine(string.Format("        {0},", clip.name));
                }

                writer.WriteLine("    }");
                writer.WriteLine("}");
                writer.Flush();
                writer.Close();
            }
        }
        #endif
    }
}
