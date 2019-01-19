using System;
using SocialGame.Internal.Network.HTTP;
using SocialGame.Internal.Network.gRPC;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace SocialGame.Internal.Network
{
    [Serializable]
    internal enum Server
    {
        REST,
        gRPC,
    }
    
    [Serializable]
    public sealed class NetworkSettings : ScriptableObject
    {
        public const string FileName = "NetworkSettings";

        [SerializeField] private Server _server = Server.REST;
        
        [SerializeField] private GeneralSettings _general = null;

        [SerializeField] private HttpSettings _http = null;

        [SerializeField] private gRPCSettings _grpc = null;
        
        internal Server Server => _server;

        internal GeneralSettings General => _general;

        internal HttpSettings Http => _http;

        internal gRPCSettings gRPC => _grpc;
        
        #if UNITY_EDITOR
        [CustomEditor(typeof(NetworkSettings))]
        private class CustomInspector : Editor
        {
            private NetworkSettings _owner;

            private void OnEnable()
            {
                _owner = target as NetworkSettings;
            }
            
            public override void OnInspectorGUI()
            {
                _owner._server = (Server)EditorGUILayout.EnumPopup("Server", _owner.Server);
                
                EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(_owner._general)), new GUIContent("General Settings"), true);

                serializedObject.Update();
                switch (_owner.Server)
                {
                    case Server.REST:
                        EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(_owner._http)), new GUIContent("Settings"), true);
                        break;
                    case Server.gRPC:
                        EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(_owner._grpc)), new GUIContent("Settings"), true);
                        break;
                    default:
                        break;
                }
                serializedObject.ApplyModifiedProperties();
            }
        }
        #endif
    }
}
