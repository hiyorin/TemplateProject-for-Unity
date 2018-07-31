using System;
using System.Linq;
using SocialGame.Dialog;
using UnityEngine;
using Zenject;
#if UNITY_EDITOR
using System.IO;
using System.Text;
using UnityEditor;
#endif

namespace SocialGame.Internal.Dialog
{
    [CreateAssetMenu(fileName = "DialogSettingsInstaller", menuName = "Installers/DialogSettingsInstaller")]
    public class DialogSettingsInstaller : ScriptableObjectInstaller<DialogSettingsInstaller>
    {
        [SerializeField] private DialogSettings _settings = null;

        public override void InstallBindings()
        {
            Container.BindInstance(_settings).AsSingle();
        }

        #if UNITY_EDITOR
        public void OnValidate()
        {
            string path = Path.Combine(Application.dataPath, ProjectModel.RootPath, "Scripts/Dialog/DialogType.cs");
            using (StreamWriter writer = new StreamWriter(path, false, Encoding.UTF8))
            {
                writer.WriteLine("// this file was auto-generated.");
                writer.WriteLine("namespace SocialGame.Dialog");
                writer.WriteLine("{");
                writer.WriteLine("    public enum DialogType");
                writer.WriteLine("    {");
                writer.WriteLine("        Sample = -1,");

                foreach (var prefab in _settings.Prefabs.Where(x => x != null))
                {
                    writer.WriteLine(string.Format("        {0},", prefab.name.Replace("Dialog", "")));
                }

                writer.WriteLine("    }");
                writer.WriteLine("}");
                writer.Flush();
                writer.Close();
            }

            AssetDatabase.Refresh(ImportAssetOptions.ImportRecursive);

            Debug.unityLogger.Log(GetType().Name, "auto-generated DialogType");
        }
        #endif
    }
}
