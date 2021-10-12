using UnityEditor;
using UnityEngine;

//[CustomPropertyDrawer(typeof(BugType))]
public class BugTypeDrawer : PropertyDrawer
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
        var pickerRect = new Rect(position.x, position.y, position.height * 2, position.height);
 
        // Draw fields - pass GUIContent.none to each so they are drawn without labels
        //EditorGUI.PropertyField(pickerRect, property.FindPropertyRelative("type"), GUIContent.none);
        //EditorGUI.ColorField(pickerRect, Color.white);

        // Set indent back to what it was
        EditorGUI.indentLevel = indent;

        EditorGUI.EndProperty();
    }
}
