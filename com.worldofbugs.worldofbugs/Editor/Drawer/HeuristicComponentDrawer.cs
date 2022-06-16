
using UnityEditor;
using UnityEngine;
using WorldOfBugs;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

[CustomPropertyDrawer(typeof(HeuristicComponent))]
public class HeuristicComponentDrawer : PropertyDrawer {

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
        // Using BeginProperty / EndProperty on the parent property means that
        // prefab override logic works on the entire property.
        EditorGUI.BeginProperty(position, label, property);

        // Draw label
        position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

        // Don't make child fields be indented
        var indent = EditorGUI.indentLevel;
        EditorGUI.indentLevel = 0;

        GameObject go = ((MonoBehaviour)property.serializedObject.targetObject).gameObject;
        HeuristicComponent[] choices = go.GetComponents<HeuristicComponent>();
        string[] s_choices = new string[] { "None" }.Concat(choices.Select(x => x.GetType().Name)).ToArray();

        // find current choice index
        int _choice = Array.FindIndex(choices, x => x == property.objectReferenceValue) + 1;

        EditorGUI.BeginChangeCheck();

        _choice = EditorGUI.Popup(position, _choice, s_choices);
        if(EditorGUI.EndChangeCheck()) {
            foreach (HeuristicComponent h in choices) {
                h.enabled = false;
            }
            Debug.Log(_choice);
            if (_choice > 0) {
                choices[_choice - 1].enabled = true; // enable only the one that is in use!
                property.objectReferenceValue = choices[_choice - 1];
            } else {
                property.objectReferenceValue = null; // no heuristic is being used, python or ML is being used.
            }
            property.serializedObject.ApplyModifiedProperties();
        }

        // Set indent back to what it was
        EditorGUI.indentLevel = indent;

        EditorGUI.EndProperty();


    }
}
