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

        [SerializeField] private Vector2Int _standardSize = new Vector2Int(1280, 720);

        [SerializeField] private Vector2Int _highSize = new Vector2Int(1920, 1080);

        [SerializeField] private float _lowRate = 0.6f;

        [SerializeField] private float _standardRate = 0.8f;

        [SerializeField] private float _highRate = 1.0f;

        [SerializeField] private Vector2Int _canvasSize = new Vector2Int(1280, 720);
        
        internal ResType Type => _type;
        
        internal Vector2Int LowSize => _lowSize;

        internal Vector2Int StandardSize => _standardSize;

        internal Vector2Int HighSize => _highSize;

        internal float LowRate => _lowRate;

        internal float StandardRate => _standardRate;

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
                        _owner._standardSize = EditorGUILayout.Vector2IntField("Standard", _owner.StandardSize);
                        _owner._highSize = EditorGUILayout.Vector2IntField("High", _owner.HighSize);
                        break;
                    case ResType.Variable:
                        _owner._lowRate = EditorGUILayout.Slider("Low", _owner.LowRate, 0.1f, 1.0f);
                        _owner._standardRate = EditorGUILayout.Slider("Standard", _owner.StandardRate, 0.1f, 1.0f);
                        _owner._highRate = EditorGUILayout.Slider("High", _owner.HighRate, 0.1f, 1.0f);
                        break;
                    default:
                        Debug.unityLogger.LogError(GetType().Name, string.Format("Not supported {0}", _owner.Type));
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
        }
        #endif
    }
}
