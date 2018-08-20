using System.IO;
using System.Linq;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityExtensions.Editor;
using Zenject;

namespace SocialGame.Internal
{
    public sealed class SetupWindow : ScriptableWizard
    {
        [MenuItem("Window/Setup SocialGameTemplate")]
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
            EditorGUILayout.LabelField("2. Create system settings");
            EditorGUILayout.LabelField("3. Create assembly definition files");
            EditorGUILayout.LabelField("4. Create unit test assembly and folder");
            return true;
        }

        private void OnWizardCreate()
        {
            SetupExtensions();
            SetupBuildSettingsScene();
            SetupSettings<ProjectSettings>("ProjectSettings");
            SetupSettings<Dialog.DialogSettingsInstaller>("DialogSettings");
            SetupSettings<Loading.LoadingSettingsInstaller>("LoadingSettings");
            SetupSettings<TapEffect.TapEffectSettingsInstaller>("TapEffectSettings");
            SetupSettings<Toast.ToastSettingsInstaller>("ToastSettings");
            SetupSettings<Transition.TransitionSettingsInstaller>("TransitionSettings");
            SetupAssemblyDefinitionFiles();
            SetupUnitTest();
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        private void SetupExtensions()
        {
            UniRxMenu.Enable();
            DOTweenMenu.Enable();
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

        private void SetupSettings<T>(string fileName) where T:ScriptableObject
        {
            string path = string.Format("Assets/Resources/{0}.asset", fileName);
            if (AssetDatabase.LoadAssetAtPath<ScriptableObjectInstaller>(path) == null)
            {
                var instance = CreateInstance<T>();
                AssetDatabase.CreateAsset(instance, path);
            }
        }

        private void SetupAssemblyDefinitionFiles()
        {
            new AssemblyDefinitionBuilder("Plugins/UniRx/Scripts/UniRx")
                .Write();
            new AssemblyDefinitionBuilder("Plugins/MessagePack/MessagePack")
                .Write();
            new AssemblyDefinitionBuilder("Plugins/UnityExtensions/UnityExtensions")
                .AddReferences("UniRx")
                .Write();
            new AssemblyDefinitionBuilder("Plugins/SocialGameTemplate/Scripts/SocialGameTemplate")
                .AddReferences(
                    "Zenject",
                    "UniRx",
                    "MessagePack",
                    "MemoryInfoPlugin",
                    "UnityExtensions")
                .Write();
        }

        private void SetupUnitTest()
        {
            Directory.CreateDirectory(Path.Combine(Application.dataPath, "Tests"));
            Directory.CreateDirectory(Path.Combine(Application.dataPath, "Tests/Editor"));
            Directory.CreateDirectory(Path.Combine(Application.dataPath, "Tests/Play"));

            new AssemblyDefinitionBuilder("Tests/Editor/Editor")
                .AddReferences(
                    "UniRx",
                    "Zenject-TestFramework-Editor",
                    "Zenject-TestFramework",
                    "Zenject",
                    "SocialGameTemplate")
                .AddIncludePlatforms("Editor")
                .EnableTestAssemblies()
                .Write();

            new AssemblyDefinitionBuilder("Tests/Play/Play")
                .AddReferences(
                    "UniRx",
                    "Zenject-TestFramework-Editor",
                    "Zenject-TestFramework",
                    "Zenject",
                    "SocialGameTemplate")
                .EnableTestAssemblies()
                .Write();
        }
    }
}
