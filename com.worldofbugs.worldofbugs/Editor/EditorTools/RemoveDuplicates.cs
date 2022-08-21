using UnityEngine;
using UnityEditor;
using System.Linq;
using System;
using System.Collections.Generic;

namespace WorldOfBugs {

    /// <summary>
    /// Removes GameObjects from selection made in the inspector that share the same position. Leaves a single object at each shared position.
    /// </summary>
    public class RemoveDuplicates : EditorWindow {
        private static readonly Vector2Int size = new Vector2Int(250, 100);

        private int index = 0;
        private Vector2 scrollPos = new Vector2(0, 0);

        [MenuItem("GameObject/Tools/RemoveDuplicates")]
        public static void ShowWindow() {
            EditorWindow window = GetWindow<RemoveDuplicates>();
            window.minSize = size;
            window.maxSize = size;
        }

        private void OnGUI() {
            //EditorGUILayout.LabelField("Remove all objects that share a position\n leaving a single version of the object at the given position.");
            EditorGUIUtility.labelWidth = 60;
            scrollPos = EditorGUILayout.BeginScrollView(scrollPos, GUILayout.Width(size[0]),
                        GUILayout.Height(size[1] - 40));
            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.Space();

            foreach(GameObject go in Selection.gameObjects) {
                if(go != null) {
                    EditorGUILayout.ObjectField("Selected:", go, typeof(GameObject), true);
                }
            }

            EditorGUI.EndDisabledGroup();
            EditorGUILayout.EndScrollView();

            if(GUILayout.Button("Remove Duplicates")) {
                //var group = Selection.gameObjects.GroupBy(x=> new {x.transform.position, x.name});
                var results = Selection.gameObjects.GroupBy(x => x.transform.position, (key,
                              g) => g.ToList());

                foreach(List<GameObject> gos in results) {
                    gos.Skip(1).ToList().ForEach(x => DestroyImmediate(x));
                }
            }
        }

        public bool GroupDuplicate(GameObject go1, GameObject go2) {
            return go1.name == go2.name; //&& go1.transform.position == go2.transform.position;
        }
    }
}
