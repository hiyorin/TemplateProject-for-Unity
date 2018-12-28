using UnityEditor;
using UnityEngine;

namespace SocialGame.Internal.Sound.Editor
{
    [CustomEditor(typeof(SoundSettingsInstaller))]
    internal class SoundSettingsInstallerEditor  : UnityEditor.Editor
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
        }
    }
}