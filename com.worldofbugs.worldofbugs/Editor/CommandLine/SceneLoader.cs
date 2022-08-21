using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using CommandLine;

namespace WorldOfBugs.Editor {

    /// <summary>
    /// Utility class that can load (and play) scenes via command line.
    ///
    /// <example>
    ///     >> <PATH_TO_UNITY_EDITOR> -
    /// </example>
    /// </summary>
    public class SceneLoader : MonoBehaviour {

        public class Options {
            [Option('s', "scene", Required = true, Default = "World-v1",
                    HelpText = "Scene to load.")]
            public string Scene {
                get;
                set;
            }
            [Option('r', "play", Required = false, Default = false,
                    HelpText = "Whether to immediately start playmode.")]
            public bool Play {
                get;
                set;
            }
        }

        public struct SceneInfo {
            public string Path;
            public SceneInfo(string path) {
                Path = path;
            }
            public string Name {
                get {
                    string[] split = Path.Split('/');
                    return split[split.Length - 1].Split('.')[0];
                }
            }
        }

        /// <summary>
        /// Method to call with -executeMethod
        ///
        /// <example>
        ///     >> ... -executeMethod SceneLoader.Main --scene World-v1 --run true
        /// </example>
        /// </summary>
        public static void Main() {
            string[] args = System.Environment.GetCommandLineArgs();
            Parser parser = new Parser(settings => {
                settings.IgnoreUnknownArguments = true;
            });
            ParserResult<Options> options = parser.ParseArguments<Options>(args);
            options.ThrowOnParseError();
            options.WithParsed(x => SceneLoader.LoadScene(x.Scene, x.Play));
        }

        [MenuItem("Test/LoadScene")]
        public static void TestLoadScene() {
            LoadScene("Maze-v0", true);
        }

        public static void LoadScene(string name, bool playmode = false) {
            LoadSceneByName(name);

            if(playmode) {
                EditorApplication.EnterPlaymode();
            }
        }

        public static void LoadSceneByName(string name) {
            SceneInfo scene = GetSceneInfo(name);
            EditorSceneManager.OpenScene(scene.Path);
        }

        public static SceneInfo GetSceneInfo(string name) {
            return GetSceneInfo(new List<string>() {
                name
            }).FirstOrDefault();
        }

        public static IEnumerable<SceneInfo> GetSceneInfo(IEnumerable<string> names) {
            IEnumerable<string> sceneGUIDs = AssetDatabase.FindAssets("t:Scene");
            IEnumerable<SceneInfo> scenes = sceneGUIDs.Select(AssetDatabase.GUIDToAssetPath).Select(
                                                x => new SceneInfo(x));
            scenes = scenes.Join(names, x => x.Name, x => x, (x, y) => x).ToList();
            scenes.ToList().ForEach(x => Debug.Log(x.Path));
            return scenes;
        }
    }
}
