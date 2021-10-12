using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
 
using UnityEditor;
using UnityEngine;
 
[CanEditMultipleObjects]
[CustomEditor(typeof(UnityEngine.Object), true)]
public class AttributeValidatorEditor : Editor {

    private bool useDefaultInspector = false;
    private IEnumerable<FieldInfo> fields;
 
    private bool shouldShowErrors = true;
 
    private void OnEnable()
    {
        fields = GetAllFields(target.GetType());
        useDefaultInspector = fields.All(f => f.GetCustomAttributes(typeof(ValidationAttribute), true).Length == 0);
    }
 
    public override void OnInspectorGUI()
    {
        if (useDefaultInspector)
        {
            base.OnInspectorGUI();
            return;
        }
 
        this.serializedObject.Update();
 
        foreach (var f in fields)
        {
            ValidateField(f);
        }
 
        shouldShowErrors = this.serializedObject.ApplyModifiedProperties();
    }
 
    private void ValidateField(FieldInfo field)
    {
        var prop = GetSerializedProperty(field);
        if (prop == null)
            return;
 
        object[] atts = field.GetCustomAttributes(typeof(ValidationAttribute), true);
        foreach (var att in atts)
        {
            ValidateAttribute(att as ValidationAttribute, field);
        }
 
        DrawProperty(prop);
    }
 
    private void DrawProperty(SerializedProperty prop)
    {
        EditorGUILayout.PropertyField(prop, true);
    }
 
    private void ValidateAttribute(ValidationAttribute attribute, FieldInfo field)
    {
        if (!attribute.Validate(field, this.target))
        {
            EditorGUILayout.HelpBox(attribute.ErrorMessage, MessageType.Error, true);
            if (shouldShowErrors)
                ShowError(attribute.ErrorMessage, this.target);
        }
    }
 
    private SerializedProperty GetSerializedProperty(FieldInfo field)
    {
        // Do not display properties marked with HideInInspector attribute
        object[] hideAtts = field.GetCustomAttributes(typeof(HideInInspector), true);
        if (hideAtts.Length > 0)
            return null;
 
        return this.serializedObject.FindProperty(field.Name);
    }
 
    public static IEnumerable<FieldInfo> GetAllFields(Type t)
    {
        if (t == null)
            return Enumerable.Empty<FieldInfo>();
 
        BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic |
                             BindingFlags.Static | BindingFlags.Instance  |
                             BindingFlags.DeclaredOnly;
 
        return t.GetFields(flags).Concat(GetAllFields(t.BaseType));
    }
 
    private static void ShowError(string msg, UnityEngine.Object o)
    {
        Debug.LogError(msg, o);
    }
 
    [UnityEditor.Callbacks.DidReloadScripts]
    private static void OnScriptReload()
    {
        MonoBehaviour[] behaviours = MonoBehaviour.FindObjectsOfType<MonoBehaviour>();
        foreach(var b in behaviours)
        {
            var fields = GetAllFields(b.GetType());
            foreach(var f in fields)
            {
                var atts = f.GetCustomAttributes(typeof(ValidationAttribute), true);
                foreach(var a in atts)
                {
                    var vatt = a as ValidationAttribute;
                    if(!vatt.Validate(f, b))
                    {
                        ShowError(vatt.ErrorMessage, b);
                    }
                }
            }
        }
    }
}


