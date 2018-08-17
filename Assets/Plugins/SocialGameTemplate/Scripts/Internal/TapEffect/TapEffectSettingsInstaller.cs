using System;
using System.Linq;
using SocialGame.TapEffect;
using UnityEngine;
using Zenject;
#if UNITY_EDITOR
using System.IO;
using System.Text;
using UnityEditor;
#endif

namespace SocialGame.Internal.TapEffect
{
    [CreateAssetMenu(fileName = "TapEffectSettingsInstaller", menuName = "Installers/TapEffectSettingsInstaller")]
    public sealed class TapEffectSettingsInstaller : ScriptableObjectInstaller<TapEffectSettingsInstaller>
    {
        [SerializeField] private TapEffectSettings _settings = null;

        public override void InstallBindings()
        {
            Container.BindInstance(_settings).AsSingle();
        }

        #if UNITY_EDITOR
        public void OnValidate()
        {
            string path = Path.Combine(Application.dataPath, ProjectModel.RootPath, "Scripts/TapEffect/TapEffectType.cs");
            using (StreamWriter writer = new StreamWriter(path, false, Encoding.UTF8))
            {
                writer.WriteLine("// this file was auto-generated.");
                writer.WriteLine("namespace SocialGame.TapEffect");
                writer.WriteLine("{");
                writer.WriteLine("    public enum TapEffectType");
                writer.WriteLine("    {");
                writer.WriteLine("        Sample = -1,");

                foreach (var prefab in _settings.Prefabs.Where(x => x != null))
                {
                    writer.WriteLine(string.Format("        {0},", prefab.name.Replace("TapEffect", "")));
                }

                writer.WriteLine("    }");
                writer.WriteLine("}");
                writer.Flush();
                writer.Close();
            }

            AssetDatabase.Refresh(ImportAssetOptions.ImportRecursive);

            Debug.unityLogger.Log(GetType().Name, "auto-generated TapEffectType");
        }
#endif
    }
}
