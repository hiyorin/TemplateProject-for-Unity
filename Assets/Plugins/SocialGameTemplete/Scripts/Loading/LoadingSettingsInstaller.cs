using System;
using System.Linq;
using UnityEngine;
using Zenject;
using UnityObject = UnityEngine.Object;
#if UNITY_EDITOR
using System.IO;
using System.Text;
using UnityEditor;
#endif

namespace SocialGame.Loading
{
    [Serializable]
    public class LoadingSettings
    {
        [SerializeField] private UnityObject [] _prefabs = null;
        [SerializeField] private float _defaultDuration = 0.5f;

        public UnityObject [] Prefabs { get { return _prefabs; } }
        public float DefaoutDuration { get { return _defaultDuration; } }
    }

    [CreateAssetMenu(fileName = "LoadingSettingsInstaller", menuName = "Installers/LoadingSettingsInstaller")]
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
            string path = Path.Combine(Application.dataPath, ProjectModel.RootPath, "Scripts/Loading/LoadingType.cs");
            using (StreamWriter writer = new StreamWriter(path, false, Encoding.UTF8))
            {
                writer.WriteLine("// this file was auto-generated.");
                writer.WriteLine("namespace SocialGame.Loading");
                writer.WriteLine("{");
                writer.WriteLine("    public enum LoadingType");
                writer.WriteLine("    {");
                writer.WriteLine("        Sample,");

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
