using System;
using System.Linq;
using SocialGame.Transition;
using UnityEngine;
using Zenject;
#if UNITY_EDITOR
using System.IO;
using System.Text;
using UnityEditor;
using UnityEditor.VersionControl;
#endif

namespace SocialGame.Internal.Transition
{
    [CreateAssetMenu(fileName = "TransitionSettings", menuName = "Installers/TransitionSettings")]
    public sealed class TransitionSettingsInstaller : ScriptableObjectInstaller<TransitionSettingsInstaller>
    {
        [SerializeField] private TransitionSettings _settings = null;

        public override void InstallBindings()
        {
            Container.BindInstance(_settings).AsSingle();
        }

        #if UNITY_EDITOR
        public void OnValidate()
        {
            if (EditorApplication.isPlaying) return;

            string fileName = Path.Combine(ProjectModel.RootPath, "Scripts/Transition/TransMode.cs");
            string filePath = Path.Combine(Application.dataPath, fileName);
            if (Provider.isActive && File.Exists(filePath))
                Provider.Checkout(Path.Combine("Assets", fileName), CheckoutMode.Asset).Wait();
            
            using (StreamWriter writer = new StreamWriter(filePath, false, Encoding.UTF8))
            {
                writer.WriteLine("// this file was auto-generated.");
                writer.WriteLine("namespace SocialGame.Transition");
                writer.WriteLine("{");
                writer.WriteLine("    public enum TransMode");
                writer.WriteLine("    {");
                writer.WriteLine("        None = -2,");
                writer.WriteLine("        BlackFade = -1,");

                foreach (var prefab in _settings.Prefabs.Where(x => x != null))
                {
                    writer.WriteLine(string.Format("        {0},", prefab.name));
                }

                writer.WriteLine("    }");
                writer.WriteLine("}");
                writer.Flush();
                writer.Close();
            }

            AssetDatabase.Refresh(ImportAssetOptions.ImportRecursive);

            Debug.unityLogger.Log(GetType().Name, "auto-generated TransMode");
        }
        #endif
    }
}
