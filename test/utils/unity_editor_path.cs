using UnityEditor;
using UnityEngine;

public class applicationPathExample
{
    [MenuItem("Examples/Location of Unity application")]
    static void appPath()
    {
        Debug.Log(EditorApplication.applicationPath);
    }
}
