﻿using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(Controller.BugOption))]
public class BugOptionDrawer : PropertyDrawer
{

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        // Using BeginProperty / EndProperty on the parent property means that
        // prefab override logic works on the entire property.
        EditorGUI.BeginProperty(position, label, property);

        // Draw label
        position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

        // Don't make child fields be indented
        var indent = EditorGUI.indentLevel;
        EditorGUI.indentLevel = 0;

        // Calculate rects
        float bugRectWidth = position.width - 40;
        var bugRect = new Rect(position.x, position.y, bugRectWidth, position.height);
        var enabledRect = new Rect(position.x + bugRectWidth + 10, position.y, 40, position.height);

        // Draw fields - pass GUIContent.none to each so they are drawn without labels
        EditorGUI.PropertyField(bugRect, property.FindPropertyRelative("bug"), GUIContent.none);
        EditorGUI.PropertyField(enabledRect, property.FindPropertyRelative("enabled"), GUIContent.none);

        // Set indent back to what it was
        EditorGUI.indentLevel = indent;

        EditorGUI.EndProperty();
    }
}