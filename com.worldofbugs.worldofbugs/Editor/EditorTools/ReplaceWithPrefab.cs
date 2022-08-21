using UnityEngine;
using UnityEditor;

namespace WorldOfBugs {

    public class ReplaceWithPrefab : EditorWindow {
        private static readonly Vector2Int size = new Vector2Int(250, 100);

        private int index = 0;
        private GameObject prefab;
        private Vector2 scrollPos = new Vector2(0, 0);
        [MenuItem("GameObject/Tools/ReplaceWithPrefab")]
        public static void ShowWindow() {
            EditorWindow window = GetWindow<ReplaceWithPrefab>();
            window.minSize = size;
            window.maxSize = size;
        }

        private void OnGUI() {
            EditorGUIUtility.labelWidth = 60;
            prefab = EditorGUILayout.ObjectField("Prefab:", prefab, typeof(GameObject),
                                                 false) as GameObject;
            scrollPos = EditorGUILayout.BeginScrollView(scrollPos, GUILayout.Width(size[0]),
                        GUILayout.Height(size[1] - 40));
            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.Space();
            GameObject[] selectedObjects = Selection.gameObjects;

            foreach(GameObject go in selectedObjects) {
                EditorGUILayout.ObjectField("Selected:", go, typeof(GameObject), true);
            }

            EditorGUI.EndDisabledGroup();
            EditorGUILayout.EndScrollView();

            if(GUILayout.Button("Replace")) {
                foreach(GameObject go in selectedObjects) {
                    Replace(prefab, go);
                }
            }
        }

        public void Replace(GameObject prefab, GameObject go) {
            if(prefab != null) {
                GameObject newObject;
                newObject = (GameObject)EditorUtility.InstantiatePrefab(prefab);
                newObject.transform.position = go.transform.position;
                newObject.transform.rotation = go.transform.rotation;
                newObject.transform.parent = go.transform.parent;
                DestroyImmediate(go);
            } else {
                throw new System.Exception("Missing prefab, could not replace selected game objects.");
            }
        }
    }
}
