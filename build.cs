using UnityEditor;
using UnityEngine;
using System.Collections;

// "C:\Program Files (x86)\Unity\Editor\Unity.exe" -projectPath "C:\Projects\svn_repos" -executeMethod MyEditorScript.Start

public class MyEditorScript: MonoBehaviour
{
    static void Start()
    {
        string[] scenes = {
        "Assets/Scenes/World-v1.unity",
        "Assets/Scenes/World-v2.unity"
        };
        
        BuildPipeline.BuildPlayer(scenes, "StandaloneWindows64", BuildTarget.StandaloneWindows64, BuildOptions.None);
    }
}