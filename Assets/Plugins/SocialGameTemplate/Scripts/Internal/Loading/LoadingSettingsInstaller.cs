using System;
using System.Linq;
using SocialGame.Loading;
using UnityEngine;
using Zenject;
#if UNITY_EDITOR
using System.IO;
using System.Text;
using UnityEditor;
using UnityEditor.VersionControl;
#endif

namespace SocialGame.Internal.Loading
{
    [CreateAssetMenu(fileName = "LoadingSettings", menuName = "Installers/LoadingSettings")]
    public class LoadingSettingsInstaller : ScriptableObjectInstaller<LoadingSettingsInstaller>
    {
        [SerializeField] private LoadingSettings _settings = null;

        public override void InstallBindings()
        {
            Container.BindInstance(_settings).AsSingle();
        }

        #if UNITY_EDITOR
        public void OnValidate()
        {
            string fileName = Path.Combine(ProjectModel.RootPath, "Scripts/Loading/LoadingType.cs");
            string filePath = Path.Combine(Application.dataPath, fileName);
            if (Provider.isActive && File.Exists(filePath))
                Provider.Checkout(Path.Combine("Assets", fileName), CheckoutMode.Asset).Wait();
            
            using (StreamWriter writer = new StreamWriter(filePath, false, Encoding.UTF8))
            {
                writer.WriteLine("// this file was auto-generated.");
                writer.WriteLine("namespace SocialGame.Loading");
                writer.WriteLine("{");
                writer.WriteLine("    public enum LoadingType");
                writer.WriteLine("    {");
                writer.WriteLine("        Sample = -1,");

                foreach (var prefab in _settings.Prefabs.Where(x => x != null))
                {
                    writer.WriteLine(string.Format("        {0},", prefab.name.Replace("Loading", "")));
                }

                writer.WriteLine("    }");
                writer.WriteLine("}");
                writer.Flush();
                writer.Close();
            }

            AssetDatabase.Refresh(ImportAssetOptions.ImportRecursive);

            Debug.unityLogger.Log(GetType().Name, "auto-generated LoadingType");
        }
#endif
    }
}
