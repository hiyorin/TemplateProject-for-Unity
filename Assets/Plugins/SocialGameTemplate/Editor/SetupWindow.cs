using System.IO;
using System.Linq;
using System.Collections.Generic;
using SocialGame.Internal.Network;
using UnityEditor;
using UnityEngine;
using UnityExtensions.Editor;
using Zenject;

namespace SocialGame.Internal.Editor
{
    internal sealed class SetupWindow : ScriptableWizard
    {
        private const string ADX2_Symbol = "SGT_ADX2";

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
            SetupSettings<ProjectSettings>(ProjectSettings.FileName);
            SetupSettings<NetworkSettings>(NetworkSettings.FileName);
            SetupSettings<ResolutionSettings>(ResolutionSettings.FileName);
            SetupSettings<Dialog.DialogSettingsInstaller>("DialogSettings");
            SetupSettings<Loading.LoadingSettingsInstaller>("LoadingSettings");
            SetupSettings<TapEffect.TapEffectSettingsInstaller>("TapEffectSettings");
            SetupSettings<Toast.ToastSettingsInstaller>("ToastSettings");
            SetupSettings<Transition.TransitionSettingsInstaller>("TransitionSettings");
            SetupSettings<Sound.SoundSettingsInstaller>("SoundSettings");
            SetupAssemblyDefinitionFiles();
            SetupUnitTest();
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            
            
            if (Directory.Exists(Path.Combine(Application.dataPath, "Plugins/CriWare")))
                MenuEditor.AddSymbols(ADX2_Symbol);
            else
                MenuEditor.RemoveSymbols(ADX2_Symbol);
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
            new AssemblyDefinitionBuilder("Plugins/UnityExtensions/UnityExtensions")
                .AddReferences(
                    "UniRx",
                    "UniRx.Async")
                .Write();

            var builder = new AssemblyDefinitionBuilder("Plugins/SocialGameTemplate/Scripts/SocialGameTemplate")
                .AddReferences(
                    "Zenject",
                    "UniRx",
                    "UniRx.Async",
                    "MemoryInfoPlugin",
                    "UnityExtensions");

            if (Directory.Exists(Path.Combine(Application.dataPath, "Plugins/CriWare")))
            {
                new AssemblyDefinitionBuilder("Plugins/CriWare/CriWare")
                    .Write();

                builder.AddReferences("CriWare");
            }

            builder.Write();
        }

        private void SetupUnitTest()
        {
            Directory.CreateDirectory(Path.Combine(Application.dataPath, "Tests"));
            Directory.CreateDirectory(Path.Combine(Application.dataPath, "Tests/Editor"));
            Directory.CreateDirectory(Path.Combine(Application.dataPath, "Tests/Play"));

            new AssemblyDefinitionBuilder("Tests/Editor/Editor")
                .AddReferences(
                    "UniRx",
                    "UniRx.Async",
                    "Zenject-TestFramework",
                    "Zenject",
                    "SocialGameTemplate")
                .AddIncludePlatforms("Editor")
                .EnableTestAssemblies()
                .Write();

            new AssemblyDefinitionBuilder("Tests/Play/Play")
                .AddReferences(
                    "UniRx",
                    "UniRx.Async",
                    "Zenject-TestFramework",
                    "Zenject",
                    "SocialGameTemplate")
                .EnableTestAssemblies()
                .Write();
        }
    }
}
