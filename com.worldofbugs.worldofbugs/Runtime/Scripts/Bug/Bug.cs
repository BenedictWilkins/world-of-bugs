using System;
using UnityEngine;

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

    
    }



}
