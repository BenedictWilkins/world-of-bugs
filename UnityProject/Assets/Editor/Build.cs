using UnityEditor;
using UnityEngine;
using System.Collections;
using System.Linq;

// /home/ben/Documents/repos/unity/editors/2019.4.25f1/Editor/Unity -projectPath "/home/ben/Documents/repos/WorldOfBugs/UnityProject" -executeMethod Build.Start
public class Build: MonoBehaviour
{
    static readonly string BUILD_LOCATION = "../worldofbugs/builds/";
    static readonly string BUILD_EXT_LINUX = ".x86_64";

    static string[] GetSceneInfo(string scene_path) {
        string[] split = scene_path.Split('/');
        string name = split[split.Length-1].Split('.')[0];
        string[] result = new string[] { scene_path, name };
        return result;
    }

    static void Start() {
        var scenesGUIDs = AssetDatabase.FindAssets("t:Scene").ToList();
        var scenesPaths = scenesGUIDs.Select(AssetDatabase.GUIDToAssetPath);
        var sceneInfo = scenesPaths.Select(GetSceneInfo);
        
        // info (path, name)
        foreach (string[] info in sceneInfo) {
            // Debug.Log(info[0]);
            BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();
            buildPlayerOptions.scenes = new[] { info[0] }; // TODO allow multiple scenes in one build!
            buildPlayerOptions.locationPathName = BUILD_LOCATION + "/" + info[1] + "/" + info[1] + BUILD_EXT_LINUX;
            // TODO build target (default to Linux... check platform!)
            buildPlayerOptions.target = BuildTarget.StandaloneLinux64;
            buildPlayerOptions.options = BuildOptions.None;
            BuildPipeline.BuildPlayer(buildPlayerOptions);
        }
        EditorApplication.Exit(0);
    }
}