using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace WorldOfBugs.Editor {

    [CustomEditor(typeof(Controller))]
    public class ControllerEditor : UnityEditor.Editor {

        SerializedProperty Agents;
        private bool bugs_expanded = true;

        void OnEnable() {
            Agents = serializedObject.FindProperty("Agents");
        }

        public override void OnInspectorGUI() {
            serializedObject.Update();
            Controller controller = ((MonoBehaviour)serializedObject.targetObject).GetComponent<Controller>();
            EditorGUILayout.PropertyField(Agents);
            bugs_expanded = EditorGUILayout.Foldout(bugs_expanded, "Bugs", true);
            if (bugs_expanded) {
                ShowBugs(controller);
            }
            EditorGUILayout.EndFoldoutHeaderGroup();
        }

        internal void ShowBugs(Controller controller) {
            EditorGUILayout.BeginVertical();
            foreach (Bug bug in controller.Bugs) {
                BugOption option = ScriptableObject.CreateInstance<BugOption>();
                option.Initialize(bug);
                SerializedObject obj = new UnityEditor.SerializedObject(option);
                SerializedProperty prop = obj.FindProperty("Bug");

                GUILayout.BeginHorizontal();
                EditorGUI.BeginChangeCheck();
                bool enabled = EditorGUILayout.Toggle(bug.enabled,  GUILayout.Width(20));
                if (EditorGUI.EndChangeCheck()) {
                    bug.enabled = enabled;
                }
                EditorGUILayout.PropertyField(prop, GUIContent.none, GUILayout.Width(EditorGUIUtility.currentViewWidth-80));
                GUILayout.EndHorizontal();
            }
            EditorGUILayout.EndVertical();
        }
    }


    internal class BugOption : ScriptableObject {
        [SerializeField]
        public Bug Bug;
        public void Initialize(Bug bug) { Bug = bug; }
    }
}
