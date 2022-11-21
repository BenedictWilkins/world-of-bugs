using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NaughtyAttributes;

namespace WorldOfBugs {

    public abstract class Bug : MonoBehaviour {

        public static readonly string RENDERTYPE_DEFAULT = "Bug";

        public string Name;
        public BugType Color = (BugType)UnityEngine.Color.white;

        [SerializeField, ReadOnly, Foldout("Bug Data")]
        protected string RenderType = RENDERTYPE_DEFAULT;

        public void Tag(GameObject gameObject) {
            Material material = gameObject.GetComponent<Renderer>().material;
            material.SetOverrideTag("RenderType", RenderType);
            material.SetColor("_BugType", (Color32) Color);
        }

        public void Tag(IEnumerable<GameObject> gameObjects) {
            foreach(GameObject go in gameObjects) {
                Tag(gameObject);
            }
        }

        public void Untag(IEnumerable<GameObject> gameObjects) {
            foreach(GameObject go in gameObjects) {
                Untag(gameObject);
            }
        }

        public void Untag(GameObject gameObject) {
            if(gameObject == null) {
                return;
            }

            Renderer renderer = gameObject.GetComponent<Renderer>();

            if(renderer == null) {
                return;
            }

            renderer.material.SetOverrideTag("RenderType",
                                             ""); // TODO old RenderType might need to be stored?
        }

        public abstract void OnEnable();
        public abstract void OnDisable();

        private static System.Random _random = new System.Random();

        public static GameObject[] GetLeafChildGameObjectsWithComponent<T>(GameObject parent,
                int n) where T : Component {
            T[] children = parent.transform.GetComponentsInChildren<T>(true);

            //children = Array.FindAll(children, x => x.GetComponent<T>() != null); // leaf children
            if(n < 0) {
                n = children.Length;
            }

            return children.OrderBy(x => _random.Next()).Take(n).Select(x =>
                    x.gameObject).ToArray();
        }

        public static GameObject[] GetLeafChildGameObjectsWithComponent<T>
        (GameObject parent) where T : Component {
            T[] children = parent.transform.GetComponentsInChildren<T>(true);
            //children = Array.FindAll(children, x => x.GetComponent<T>() != null); // leaf children
            return children.OrderBy(x => _random.Next()).Select(x => x.gameObject).ToArray();
        }


#if UNITY_EDITOR
        // Debugging stuff...

        public void HighlightGameObjects(IEnumerable<GameObject> gos, Color? color = null) {
            if(color == null) {
                color = (Color)Color;
            }

            if(gos == null) {
                return;
            }

            //Debug.Log($"{gos.First()} {color}");
            foreach(GameObject go in gos) {
                MeshFilter[] mfs = go.GetComponents<MeshFilter>();

                foreach(MeshFilter mf in mfs) {
                    Color _old = Gizmos.color;
                    Gizmos.color = (Color)color;
                    Gizmos.DrawWireMesh(mf.mesh, 0, go.transform.position, go.transform.rotation,
                                        go.transform.localScale);
                    Gizmos.color = _old;
                }
            }
        }

        public void HighlightGameObject(GameObject go) {
            if(go == null) {
                return;
            }

            HighlightGameObjects(new GameObject[] {go});
        }
#endif


    }


}
