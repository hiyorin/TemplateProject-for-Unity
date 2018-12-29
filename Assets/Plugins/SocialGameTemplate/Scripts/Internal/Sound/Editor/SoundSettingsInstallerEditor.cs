using System.IO;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using SocialGame.Sound;
using UnityEditor;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityExtensions;

namespace SocialGame.Internal.Sound.Editor
{
    [CustomEditor(typeof(SoundSettingsInstaller))]
    internal class SoundSettingsInstallerEditor : UnityEditor.Editor
    {
        private SoundSettingsInstaller _owner;

        private void OnEnable()
        {
            _owner = target as SoundSettingsInstaller;
        }

        public override void OnInspectorGUI()
        {
#if SGT_ADX2
                _owner.Type = (Type)EditorGUILayout.EnumPopup(typeof(Type).Name, _owner.Type);
#else
                _owner.Type = Type.Unity;
#endif     
            serializedObject.Update();

            EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(_owner.General)), true);

            string masterFieldName = _owner.Type == Type.Unity ? nameof(_owner.UnityMaster) : nameof(_owner.Adx2Master);
            string bgmFieldName = _owner.Type == Type.Unity ? nameof(_owner.UnityBgm) : nameof(_owner.Adx2Bgm);
            string seFieldName = _owner.Type == Type.Unity ? nameof(_owner.UnitySe) : nameof(_owner.Adx2Se);
            string voiceFieldName = _owner.Type == Type.Unity ? nameof(_owner.UnityVoice) : nameof(_owner.Adx2Voice);
                
            EditorGUILayout.PropertyField(serializedObject.FindProperty(masterFieldName), new GUIContent("Master"), true);
            EditorGUILayout.PropertyField(serializedObject.FindProperty(bgmFieldName), new GUIContent("BGM"), true);
            EditorGUILayout.PropertyField(serializedObject.FindProperty(seFieldName), new GUIContent("SE"), true);
            EditorGUILayout.PropertyField(serializedObject.FindProperty(voiceFieldName), new GUIContent("Voice"), true);

            serializedObject.ApplyModifiedProperties();
            
            EditorGUILayout.Space();

            EditorGUI.BeginDisabledGroup(EditorApplication.isPlaying || EditorApplication.isCompiling);
            bool clickGenerateButton = GUILayout.Button("Generate Enum");
            EditorGUI.EndDisabledGroup();
            if (clickGenerateButton)
            {
                bool refresh = false;
                IEnumerable<string> bgmNames = null;
                IEnumerable<string> seNames = null;
                IEnumerable<string> voiceNames = null;
                switch (_owner.Type)
                {
                    case Type.Unity:
                        bgmNames = _owner.UnityBgm.Clips.Where(x => x != null).Select(x => x.name);
                        seNames = _owner.UnitySe.Clips.Where(x => x != null).Select(x => x.name);
                        voiceNames = _owner.UnityVoice.Clips.Where(x => x != null).Select(x => x.name);
                        break;
                    case Type.ADX2:
#if SGT_ADX2
                        ADX2.ADX2Utility.Initialize();
                        bgmNames = ADX2.ADX2Utility.GetCueInfoList(_owner.Adx2Bgm.BuiltInCueSheet)?.Select(x => x.name);
                        seNames = ADX2.ADX2Utility.GetCueInfoList(_owner.Adx2Se.BuiltInCueSheet)?.Select(x => x.name);
                        voiceNames = ADX2.ADX2Utility.GetCueInfoList(_owner.Adx2Voice.BuiltInCueSheet)?.Select(x => x.name);
                        ADX2.ADX2Utility.Finalize();
#endif
                        break;
                    default:
                        Debug.unityLogger.LogError(GetType().Name, $"Not supported {_owner.Type}");
                        break;
                }
            
                refresh |= Generate(typeof(BGM).Name, bgmNames);
                refresh |= Generate(typeof(SE).Name, seNames);
                refresh |= Generate(typeof(Voice).Name, voiceNames);
                if (!refresh)
                    return;
            
                AssetDatabase.Refresh(ImportAssetOptions.ImportRecursive);

                Debug.unityLogger.Log(GetType().Name, "auto-generated Sound");
            }
        }
        
        private bool Generate(string type, IEnumerable<string> names)
        {
            if (names == null)
                return false;
            
            string fileName = Path.Combine(ProjectModel.RootPath, "Scripts/Sound", type + ".cs");
            string filePath = Path.Combine(Application.dataPath, fileName);
            
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("// this file was auto-generated.");
            builder.AppendLine("namespace SocialGame.Sound");
            builder.AppendLine("{");
            builder.AppendLine("    public enum " + type);
            builder.AppendLine("    {");

            foreach (var name in names.Where(x => !string.IsNullOrEmpty(x)))
            {
                builder.AppendLineFormat("        {0},", name);
            }
            
            builder.AppendLine("    }");
            builder.AppendLine("}");            
            
            string text = builder.ToString();
            if (File.Exists(filePath))
            {
                if (File.ReadAllText(filePath) == text)
                    return false;
                
                if (Provider.isActive)
                    Provider.Checkout(Path.Combine("Assets", fileName), CheckoutMode.Asset).Wait();
            }
            
            File.WriteAllText(filePath, text, Encoding.UTF8);
            return true;
        }
    }
}