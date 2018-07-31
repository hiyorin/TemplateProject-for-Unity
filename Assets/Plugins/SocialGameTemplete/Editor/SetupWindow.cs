using System.Linq;
using System.Collections.Generic;
using UnityEditor;
using Zenject;

namespace SocialGame.Internal
{
    public sealed class SetupWindow : ScriptableWizard
    {
        [MenuItem("Window/Setup SocialGameTemplete")]
        private static void Open()
        {
            DisplayWizard<SetupWindow>("Setup");
        }

        [InitializeOnLoadMethod]
        private static void InitializeOnLoadMethod()
        {
            var showKey = typeof(SetupWindow).Name;
            var show = EditorPrefs.GetBool(showKey, false);
            if (!show)
            {
                EditorPrefs.SetBool(showKey, true);
                Open();
            }
        }

        protected override bool DrawWizardGUI()
        {
            EditorGUILayout.LabelField("1. Setup system scenes");
            return true;
        }

        private void OnWizardCreate()
        {
            SetupBuildSettingsScene();
            SetupSettings<Dialog.DialogSettingsInstaller>("DialogSettings");
            SetupSettings<Loading.LoadingSettingsInstaller>("LoadingSettings");
            SetupSettings<TapEffect.TapEffectSettingsInstaller>("TapEffectSettings");
            SetupSettings<Toast.ToastSettingsInstaller>("ToastSettings");
            SetupSettings<Transition.TransitionSettingsInstaller>("TransitionSettings");
            AssetDatabase.SaveAssets();
        }

        private void SetupBuildSettingsScene()
        {
            List<EditorBuildSettingsScene> scenes = new List<EditorBuildSettingsScene>(EditorBuildSettings.scenes);
            foreach (var systemSceneName in ProjectModel.SystemSceneNames)
            {
                var path = string.Format("Assets/{0}/Scenes/{1}.unity", ProjectModel.RootPath, systemSceneName);
                if (!scenes.Any(x => x.path == path))
                {
                    scenes.Add(new EditorBuildSettingsScene(path, true));
                }
            }
            EditorBuildSettings.scenes = scenes.ToArray();   
        }

        private void SetupSettings<T>(string fileName) where T:ScriptableObjectInstaller
        {
            string path = string.Format("Assets/Resources/{0}.asset", fileName);
            if (AssetDatabase.LoadAssetAtPath<ScriptableObjectInstaller>(path) == null)
            {
                var instance = CreateInstance<T>();
                AssetDatabase.CreateAsset(instance, path);
            }
        }
    }
}
