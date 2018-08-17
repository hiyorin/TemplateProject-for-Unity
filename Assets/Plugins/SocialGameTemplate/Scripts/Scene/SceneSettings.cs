using UnityEngine;
using System;
using System.Linq;
using System.Collections.Generic;
using SocialGame.Internal;
#if UNITY_EDITOR
using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
#endif

namespace SocialGame.Scene
{
    public sealed class SceneSettings : MonoBehaviour
    {
        [Serializable]
        private class DataSet
        {
            public string Name;
            public bool IsActive;
            public DataSet(string name)
            {
                Name = name;
                IsActive = true;
            }
        }

        [SerializeField]
        private List<DataSet> _subs = new List<DataSet>();

        public IEnumerable<string> Subs { get { return _subs.Select(x => x.Name); } }

        public bool GetActive(string sceneName)
        {
            var current = _subs.FirstOrDefault(x => x.Name == sceneName);
            if (current == null)
            {
                return false;
            }
            return current.IsActive;
        }

        #if UNITY_EDITOR
        [CustomEditor(typeof(SceneSettings))]
        private sealed class CustomInspector : Editor
        {
            private SceneSettings _owner;

            private void OnEnable()
            {
                _owner = target as SceneSettings;
            }

            public override void OnInspectorGUI()
            {
                bool isDirty = false;

                EditorGUILayout.LabelField("Load sub scenes");
                EditorGUI.indentLevel++;
                foreach (var scene in EditorBuildSettings.scenes.Where(x => x.enabled && x.path.IndexOf(Path.Combine(ProjectModel.RootPath, "Scenes")) == -1))
                {
                    string sceneName = System.IO.Path.GetFileName(scene.path);
                    sceneName = sceneName.Replace(".unity", "");

                    bool before = _owner._subs.Any(x => x.Name == sceneName);
                    bool after = EditorGUILayout.Toggle(sceneName, before);
                    if (before != after)
                    {
                        isDirty = true;
                        if (after)
                            _owner._subs.Add(new DataSet(sceneName));
                        else
                            _owner._subs.Remove(_owner._subs.FirstOrDefault(x => x.Name == sceneName));
                    }
                }
                EditorGUI.indentLevel--;

                EditorGUILayout.LabelField("Active scenes");
                EditorGUI.indentLevel++;
                foreach(var sub in _owner._subs)
                {
                    EditorGUILayout.BeginHorizontal();
                    bool after = EditorGUILayout.Toggle(sub.Name, sub.IsActive);
                    if (sub.IsActive != after)
                    {
                        isDirty = true;
                        sub.IsActive = after;
                    }

                    var buildScene = EditorBuildSettings.scenes.FirstOrDefault(x => x.path.EndsWith(string.Format("/{0}.unity", sub.Name)));
                    if (buildScene == null)
                        continue;
                    
                    var scene = EditorSceneManager.GetSceneByName(sub.Name);
                    if (scene.isLoaded && GUILayout.Button("Unload"))
                        EditorSceneManager.CloseScene(scene, true);
                    
                    if (!scene.isLoaded && GUILayout.Button("Load"))
                        EditorSceneManager.OpenScene(buildScene.path, OpenSceneMode.Additive);

                    EditorGUILayout.EndHorizontal();
                }
                EditorGUI.indentLevel--;

                if (isDirty)
                {
                    EditorSceneManager.MarkSceneDirty(_owner.gameObject.scene);
                }
            }
        }
        #endif
    }
}
