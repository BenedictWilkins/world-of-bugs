using System;
using UnityEngine;
using System.Linq;

namespace WorldOfBugs {
    public abstract class Bug : MonoBehaviour {
        
        [SerializeField]
        public string bugTag = "Bug";
        [SerializeField] 
        public BugType bugType; 
        
        public void Tag(GameObject go) {
            Material material = go.GetComponent<Renderer>().material;
            material.SetOverrideTag("RenderType", bugTag);
            material.SetColor("_BugType", (Color32) bugType);
        }

        public void Untag(GameObject go) {
            Material material = go.GetComponent<Renderer>().material;
            material.SetOverrideTag("RenderType", "");
        }

        public abstract void OnEnable();
        public abstract void OnDisable();


        
        private static System.Random _random = new System.Random();
        
        public static GameObject[] GetLeafChildGameObjectsWithComponent<T>(GameObject parent, int n) where T : Component {
            T[] children = parent.transform.GetComponentsInChildren<T>(true);
            //children = Array.FindAll(children, x => x.GetComponent<T>() != null); // leaf children
            if (n < 0) { n = children.Length; }
            return children.OrderBy(x => _random.Next()).Take(n).Select(x => x.gameObject).ToArray();
        }

        public static GameObject[] GetLeafChildGameObjectsWithComponent<T>(GameObject parent) where T : Component {
            T[] children = parent.transform.GetComponentsInChildren<T>(true);
            //children = Array.FindAll(children, x => x.GetComponent<T>() != null); // leaf children
            return children.OrderBy(x => _random.Next()).Select(x => x.gameObject).ToArray();
        }
    }



}
