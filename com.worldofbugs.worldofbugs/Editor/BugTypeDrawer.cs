using UnityEditor;
using UnityEngine;

namespace WorldOfBugs {
    namespace Editor {
        [CustomPropertyDrawer(typeof(BugType))]
        public class BugTypeDrawer : PropertyDrawer {

            public override void OnGUI(Rect position, SerializedProperty property,
                                       GUIContent label) {
                EditorGUI.BeginProperty(position, label, property);
                position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive),
                                                 label);
                EditorGUI.PropertyField(position, property.FindPropertyRelative("type"),
                                        GUIContent.none);
                EditorGUI.EndProperty();
            }
        }
    }
}
