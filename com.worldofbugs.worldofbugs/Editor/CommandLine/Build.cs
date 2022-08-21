using UnityEditor;
using UnityEngine;

using System.Linq;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;

// command line utils https://github.com/commandlineparser/commandline
using CommandLine;
using CommandLine.Text;

//static readonly string BUILD_LOCATION = "../worldofbugs/builds/";
//static readonly string BUILD_EXT_LINUX = ".x86_64";
namespace WorldOfBugs.Editor {


    public class Build: MonoBehaviour {
        public class Options {

            [Option('b', "build-location", Required = false, Default = "./Builds",
                    HelpText = "Directory in which to place builds.")]
            public string BuildLocation {
                get;
                set;
            }

            [Option('t', "target", Required = false, Default = "StandaloneLinux64",
                    HelpText = "Target platform, see UnityEngine.BuildTarget for options.")]
            public string BuildTarget {
                get;
                set;
            }

            [Option('s', "scenes", Min = 1, Required = false,
                    HelpText =
                        "Scenes to build. If a build requires multiple scenes, specify them here as you would in the Unity editor build options.")]
            public IEnumerable<string> Scenes {
                get;
                set;
            }

            [Option('n', "name", Required = false, Default = null,
                    HelpText =
                        "Name of the build. This name is used as the WOB environment id it should follow the gym standard <NAME>-<VERSION>. Defaults to the name of the first scene provided.")]
            public string Name {
                get;
                set;
            }

            [Option('a', "all", Required = false, Default = false,
                    HelpText =
                        "Build all scenes in the open project individually, treating each scene as its own WOB environment.")]
            public bool All {
                get;
                set;
            }

            // all unknown options are ignored.
        }

        static void Start() {
            string[] args = System.Environment.GetCommandLineArgs();
            Parser parser = new Parser(settings => {
                settings.IgnoreUnknownArguments = true;
            });
            ParserResult<Options> options = parser.ParseArguments<Options>(args);
            options.ThrowOnParseError();
            options.WithParsed(Build.Parse);
            //EditorApplication.Exit(0);
        }

        static void CreateBuild(string name, string buildLocation, BuildTarget target,
                                IEnumerable<SceneLoader.SceneInfo> scenes) {
            if(name is null) {
                name = scenes.First().Name;
            }

            Debug.Log($"Creating build: {name}");
            BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();
            buildPlayerOptions.scenes = scenes.Select(x => x.Path).ToArray();
            buildPlayerOptions.locationPathName = buildLocation + "/" + name + "/" + name;
            buildPlayerOptions.target = BuildTarget.StandaloneLinux64;
            buildPlayerOptions.options = BuildOptions.None;
            BuildPipeline.BuildPlayer(buildPlayerOptions);
        }

        static void Parse(Options options) {
            BuildTarget target = (BuildTarget)Enum.Parse(typeof(BuildTarget), options.BuildTarget);
            IEnumerable<SceneLoader.SceneInfo> scenes = SceneLoader.GetSceneInfo(options.Scenes);

            if(options.All) {  // build all scenes in project individually assuming they are each a full environment.
                foreach(SceneLoader.SceneInfo scene in scenes) {
                    CreateBuild(null, options.BuildLocation, target, new SceneLoader.SceneInfo[] { scene }.ToList());
                }
            } else { // build supplied scenes as a single environment
                CreateBuild(options.Name, options.BuildLocation, target, scenes);
            }
        }
    }
}
