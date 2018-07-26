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

namespace SocialGame.Toast
{
    [Serializable]
    public class ToastSettings
    {
        [SerializeField] private UnityObject [] _prefabs = null;
        [SerializeField] private Color _maskColor = Color.white;
        [SerializeField] private float _defaoutDuration = 0.5f;
        [SerializeField] private float _showDuration = 2.0f;

        public UnityObject [] Prefabs { get { return _prefabs; } }
        public Color MaskColor { get { return _maskColor; } }
        public float DefaoutDuration { get { return _defaoutDuration; } }
        public float ShowDuration { get { return _showDuration; } }
    }

    [CreateAssetMenu(fileName = "ToastSettingsInstaller", menuName = "Installers/ToastSettingsInstaller")]
    public class ToastSettingsInstaller : ScriptableObjectInstaller<ToastSettingsInstaller>
    {
        [SerializeField] private ToastSettings _settings = null;

        public override void InstallBindings()
        {
            Container.BindInstance(_settings).AsSingle();
        }

        #if UNITY_EDITOR
        public void OnValidate()
        {
            string path = Path.Combine(Application.dataPath, ProjectModel.RootPath, "Scripts/Toast/ToastType.cs");
            using (StreamWriter writer = new StreamWriter(path, false, Encoding.UTF8))
            {
                writer.WriteLine("// this file was auto-generated.");
                writer.WriteLine("namespace SocialGame.Toast");
                writer.WriteLine("{");
                writer.WriteLine("    public enum ToastType");
                writer.WriteLine("    {");
                writer.WriteLine("        Sample = -1,");

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

            Debug.unityLogger.Log(GetType().Name, "auto-generated ToastType");
        }
#endif
    }
}
