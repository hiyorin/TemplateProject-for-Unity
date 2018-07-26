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

namespace SocialGame.Transition
{
    [Serializable]
    public class TransitionSettings
    {
        [SerializeField] private UnityObject[] _prefabs = null;
        [SerializeField] private float _defaultDuration = 0.5f;

        public UnityObject[] Prefabs { get { return _prefabs; } }
        public float DefaultDuration { get { return _defaultDuration; } }
    }

    [CreateAssetMenu(fileName = "TransitionSettingsInstaller", menuName = "Installers/TransitionSettingsInstaller")]
    public class TransitionSettingsInstaller : ScriptableObjectInstaller<TransitionSettingsInstaller>
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

            string path = Path.Combine(Application.dataPath, ProjectModel.RootPath, "Scripts/Transition/TransMode.cs");
            using (StreamWriter writer = new StreamWriter(path, false, Encoding.UTF8))
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
