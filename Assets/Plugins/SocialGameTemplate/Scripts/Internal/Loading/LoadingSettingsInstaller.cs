using System;
using System.Linq;
using SocialGame.Loading;
using UnityEngine;
using UnityExtensions;
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
            if (EditorApplication.isPlaying)
                return;
            
            string fileName = Path.Combine(ProjectModel.RootPath, "Scripts/Loading/LoadingType.cs");
            string filePath = Path.Combine(Application.dataPath, fileName);
            
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("// this file was auto-generated.");
            builder.AppendLine("namespace SocialGame.Loading");
            builder.AppendLine("{");
            builder.AppendLine("    public enum LoadingType");
            builder.AppendLine("    {");
            builder.AppendLine("        Sample = -1,");
            
            foreach (var prefab in _settings.Prefabs.Where(x => x != null))
            {
                builder.AppendLineFormat("        {0},", prefab.name.Replace("Loading", ""));
            }
            
            builder.AppendLine("    }");
            builder.AppendLine("}");
            
            string text = builder.ToString();
            if (File.Exists(filePath))
            {
                if (File.ReadAllText(filePath) == text)
                    return;
                
                if (Provider.isActive)
                    Provider.Checkout(Path.Combine("Assets", fileName), CheckoutMode.Asset).Wait();
            }

            File.WriteAllText(filePath, text, Encoding.UTF8);
            
            AssetDatabase.Refresh(ImportAssetOptions.ImportRecursive);

            Debug.unityLogger.Log(GetType().Name, "auto-generated LoadingType");
        }
#endif
    }
}
