#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Text;
using System.Linq;

namespace SocialGame.Scene
{
    public static class SceneEditor
    {
        [InitializeOnLoadMethod]
        private static void InitializeOnLoadMethod()
        {
            CreateSceneEnum();
            EditorBuildSettings.sceneListChanged += CreateSceneEnum;
        }

        private static void CreateSceneEnum()
        {
            string path = Path.Combine(Application.dataPath, ProjectModel.RootPath, "Scripts/Scene/Scene.cs");
            using (StreamWriter writer = new StreamWriter(path, false, Encoding.UTF8))
            {
                writer.WriteLine("// this file was auto-generated.");
                writer.WriteLine("namespace SocialGame.Scene");
                writer.WriteLine("{");
                writer.WriteLine("    public enum Scene");
                writer.WriteLine("    {");
                
                foreach (string scenePath in EditorBuildSettings.scenes
                    .Where(x => x.enabled)
                    .Select(x => x.path)
                    .Where(x => !ProjectModel.SystemSceneNames.Any(y => x.EndsWith(string.Format("/{0}.unity", y))))
                    .Where(x => x.IndexOf("/SocialGameTemplete/Examples") < 0))
                {
                    string sceneName = Path.GetFileName(scenePath);
                    sceneName = sceneName.Replace(".unity", "");
                    writer.WriteLine(string.Format("        {0},", sceneName));
                }

                writer.WriteLine("    }");
                writer.WriteLine("}");
                writer.Flush();
                writer.Close();
            }

            AssetDatabase.Refresh(ImportAssetOptions.ImportRecursive);

            Debug.unityLogger.Log(typeof(SceneEditor).Name, "auto-generated SceneEnum");
        }
    }
}
#endif
