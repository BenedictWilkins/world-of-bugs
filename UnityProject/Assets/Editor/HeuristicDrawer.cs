
using UnityEditor;
using UnityEngine;
using WorldOfBugs;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

[CustomPropertyDrawer(typeof(Heuristic))]
public class HeuristicDrawer : PropertyDrawer {
    
    private int _choice = 0;

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
        Heuristic[] choices = go.GetComponents<Heuristic>();
        string[] s_choices = choices.Select(x => x.GetType().Name).ToArray();
        
        // find current choice index
        int _choice = Array.FindIndex(choices, x => x == property.objectReferenceValue);
    
        EditorGUI.BeginChangeCheck();

        _choice = EditorGUI.Popup(position, _choice, s_choices);
        if(EditorGUI.EndChangeCheck()) {
            foreach (Heuristic h in choices) {
                h.enabled = false;
            }
            choices[_choice].enabled = true; // enable only the one that is in use!
            property.objectReferenceValue = choices[_choice];
            property.serializedObject.ApplyModifiedProperties();
        }

        // Set indent back to what it was
        EditorGUI.indentLevel = indent;

        EditorGUI.EndProperty();

        
    }
}