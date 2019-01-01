using SocialGame.Internal.Data.DataStore;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace SocialGame.Internal
{
    public sealed class ResolutionSettings : ScriptableObject
    {
        internal enum ResType
        {
            Static,
            Variable,
        }

        public const string FileName = "ResolutionSettings";
        
        [SerializeField] private ResType _type;
        
        [SerializeField] private Vector2Int _lowSize = new Vector2Int(1136, 640);

        [SerializeField] private Vector2Int _middleSize = new Vector2Int(1280, 720);

        [SerializeField] private Vector2Int _highSize = new Vector2Int(1920, 1080);

        [SerializeField] private float _lowRate = 0.6f;

        [SerializeField] private float _middleRate = 0.8f;

        [SerializeField] private float _highRate = 1.0f;

        [SerializeField] private Vector2Int _canvasSize = new Vector2Int(1280, 720);
        
        internal ResType Type => _type;
        
        internal Vector2Int LowSize => _lowSize;

        internal Vector2Int MiddleSize => _middleSize;

        internal Vector2Int HighSize => _highSize;

        internal float LowRate => _lowRate;

        internal float MiddleRate => _middleRate;

        internal float HighRate => _highRate;

        internal Vector2Int CanvasSize => _canvasSize;
        
        #if UNITY_EDITOR
        [CustomEditor(typeof(ResolutionSettings))]
        private class CustomInspector : Editor
        {
            private ResolutionSettings _owner;

            private void OnEnable()
            {
                _owner = target as ResolutionSettings;
            }

            public override void OnInspectorGUI()
            {
                ScreenResolutionGUI();
                EditorGUILayout.Space();
                CanvasResolutionGUI();
                EditorGUILayout.Space();
                DeleteSaveFile();
                EditorUtility.SetDirty(_owner);
            }

            private void ScreenResolutionGUI()
            {
                EditorGUILayout.LabelField("Screen");
                EditorGUI.indentLevel++;
                
                _owner._type = (ResType) EditorGUILayout.EnumPopup("Type", _owner.Type);

                switch (_owner.Type)
                {
                    case ResType.Static:
                        _owner._lowSize = EditorGUILayout.Vector2IntField("Low", _owner.LowSize);
                        _owner._middleSize = EditorGUILayout.Vector2IntField("Middle", _owner.MiddleSize);
                        _owner._highSize = EditorGUILayout.Vector2IntField("High", _owner.HighSize);
                        break;
                    case ResType.Variable:
                        _owner._lowRate = EditorGUILayout.Slider("Low", _owner.LowRate, 0.1f, 1.0f);
                        _owner._middleRate = EditorGUILayout.Slider("Middle", _owner.MiddleRate, 0.1f, 1.0f);
                        _owner._highRate = EditorGUILayout.Slider("High", _owner.HighRate, 0.1f, 1.0f);
                        break;
                    default:
                        Debug.unityLogger.LogError(GetType().Name, $"Not supported {_owner.Type}");
                        break;
                }

                EditorGUI.indentLevel--;
            }
            
            private void CanvasResolutionGUI()
            {
                EditorGUILayout.LabelField("Canvas");
                EditorGUI.indentLevel++;

                _owner._canvasSize = EditorGUILayout.Vector2IntField("Size", _owner._canvasSize);

                EditorGUI.indentLevel--;
            }
            
            private void DeleteSaveFile()
            {
                if (GUILayout.Button("Delete Save File"))
                {
                    ResolutionLocalStorage
                        .DeleteFile<ResolutionLocalStorage, SocialGame.Internal.Data.Entity.Resolution>();
                }
            }
        }
        #endif
    }
}
