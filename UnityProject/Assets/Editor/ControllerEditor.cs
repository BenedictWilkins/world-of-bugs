using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

//[CustomEditor(typeof(Controller))]
public class ControllerEditor : Editor {

    SerializedProperty player;
    SerializedProperty bugs;

    void OnEnable() {
        player = serializedObject.FindProperty("player");
        bugs = serializedObject.FindProperty("bugs");
    }

    public override void OnInspectorGUI() {
        serializedObject.Update();
        EditorGUILayout.PropertyField(player);
    }
}
