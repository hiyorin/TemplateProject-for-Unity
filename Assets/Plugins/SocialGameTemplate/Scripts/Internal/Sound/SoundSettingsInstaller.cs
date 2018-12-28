using System;
using System.Linq;
using System.Collections.Generic;
using SocialGame.Sound;
using UnityEngine;
using UnityExtensions;
using Zenject;
#if UNITY_EDITOR
using System.IO;
using System.Text;
using UnityEditor;
using UnityEditor.VersionControl;
using UnityExtensions.Editor;
#endif

namespace SocialGame.Internal.Sound
{
    internal enum Type
    {
        Unity,
        ADX2,
    }
    
    [CreateAssetMenu(fileName = "SoundSettings", menuName = "Installers/SoundSettings")]
    public sealed class SoundSettingsInstaller : ScriptableObjectInstaller<SoundSettingsInstaller>
    {
        private const string Symbol = "SGT_ADX2";
        
        [SerializeField] private Type _type = Type.Unity;
        
        [SerializeField] private GeneralSettings _general = null;

        [SerializeField] private Unity.UnityMasterSettings _unityMaster = null;
        
        [SerializeField] private Unity.UnityBGMSettings _unityBgm = null;
        
        [SerializeField] private Unity.UnitySESettings _unitySe = null;

        [SerializeField] private Unity.UnityVoiceSettings _unityVoice = null;

        [SerializeField] private ADX2.ADX2MasterSettings _adx2Master;
        
        [SerializeField] private ADX2.ADX2BGMSettings _adx2Bgm = null;

        [SerializeField] private ADX2.ADX2SESettings _adx2Se = null;

        [SerializeField] private ADX2.ADX2VoiceSettings _adx2Voice;
        
        public override void InstallBindings()
        {
            Container.BindInstance(_general).AsSingle();
            
            switch (_type)
            {
                case Type.Unity:
                    Container.BindInstance(_unityMaster).AsSingle();
                    Container.BindInstance(_unityBgm).AsSingle();
                    Container.BindInstance(_unitySe).AsSingle();
                    Container.BindInstance(_unityVoice).AsSingle();
                    break;
                case Type.ADX2:
                    Container.BindInstance(_adx2Master).AsSingle();
                    Container.BindInstance(_adx2Bgm).AsSingle();
                    Container.BindInstance(_adx2Se).AsSingle();
                    Container.BindInstance(_adx2Voice).AsSingle();
                    break;
                default:
                    Debug.unityLogger.LogError(GetType().Name, $"Not supported {_type}");
                    break;
            }
        }

        #if UNITY_EDITOR
        public void OnValidate()
        {
            if (EditorApplication.isPlaying)
                return;

            UpdateSymbol();
            
            bool refresh = false;
            refresh |= Generate(typeof(BGM).Name, _unityBgm.Clips);
            refresh |= Generate(typeof(SE).Name, _unitySe.Clips);
            refresh |= Generate(typeof(Voice).Name, _unityVoice.Clips);
            if (!refresh)
                return;
            
            AssetDatabase.Refresh(ImportAssetOptions.ImportRecursive);

            Debug.unityLogger.Log(GetType().Name, "auto-generated Sound");
        }

        private void UpdateSymbol()
        {
            switch (_type)
            {
                case Type.Unity:
                    MenuEditor.RemoveSymbols(Symbol);
                    break;
                case Type.ADX2:
                    MenuEditor.AddSymbols(Symbol);
                    break;
                default:
                    MenuEditor.RemoveSymbols(Symbol);
                    break;
            }
        }
        
        private bool Generate(string type, IEnumerable<AudioClip> clips)
        {
            string fileName = Path.Combine(ProjectModel.RootPath, "Scripts/Sound", type + ".cs");
            string filePath = Path.Combine(Application.dataPath, fileName);
            
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("// this file was auto-generated.");
            builder.AppendLine("namespace SocialGame.Sound");
            builder.AppendLine("{");
            builder.AppendLine("    public enum " + type);
            builder.AppendLine("    {");

            foreach (var clip in clips.Where(x => x != null))
            {
                builder.AppendLineFormat("        {0},", clip.name);
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

        [CustomEditor(typeof(SoundSettingsInstaller))]
        private class CustomInspector : Editor
        {
            private SoundSettingsInstaller _owner;

            private void OnEnable()
            {
                _owner = target as SoundSettingsInstaller;
            }

            public override void OnInspectorGUI()
            {
                if (System.Type.GetType("CriWare, CriWare, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null") != null)
                    _owner._type = (Type)EditorGUILayout.EnumPopup(typeof(Type).Name, _owner._type);
                else
                    _owner._type = Type.Unity;
                
                serializedObject.Update();

                EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(_owner._general)), true);

                string masterFieldName = _owner._type == Type.Unity ? nameof(_owner._unityMaster) : nameof(_adx2Master);
                string bgmFieldName = _owner._type == Type.Unity ? nameof(_owner._unityBgm) : nameof(_owner._adx2Bgm);
                string seFieldName = _owner._type == Type.Unity ? nameof(_owner._unitySe) : nameof(_owner._adx2Se);
                string voiceFieldName = _owner._type == Type.Unity ? nameof(_owner._unityVoice) : nameof(_owner._adx2Voice);
                
                EditorGUILayout.PropertyField(serializedObject.FindProperty(masterFieldName), new GUIContent("Master"), true);
                EditorGUILayout.PropertyField(serializedObject.FindProperty(bgmFieldName), new GUIContent("BGM"), true);
                EditorGUILayout.PropertyField(serializedObject.FindProperty(seFieldName), new GUIContent("SE"), true);
                EditorGUILayout.PropertyField(serializedObject.FindProperty(voiceFieldName), new GUIContent("Voice"), true);

                serializedObject.ApplyModifiedProperties();
            }
        }
        #endif
    }
}
