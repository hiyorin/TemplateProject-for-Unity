#if UNITY_EDITOR
using UnityEditor;
using Zenject;

namespace SocialGame.Internal
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(SystemSceneContext))]
    public sealed class SystemSceneContextEditor : SceneContextEditor
    {
        private SerializedProperty _settingsPath;

        public override void OnEnable()
        {
            base.OnEnable();

            _settingsPath = serializedObject.FindProperty("_settingsPath");
        }

        protected override void OnGui()
        {
            base.OnGui();

            EditorGUILayout.PropertyField(_settingsPath, true);
        }
    }
}
#endif
