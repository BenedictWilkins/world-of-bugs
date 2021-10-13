using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BugTag : MonoBehaviour {  

    [SerializeField]
    public BugType bugType; 
    public string bugTag = "Bug";

    public void Tag(GameObject go) {
        Material material = go.GetComponent<Renderer>().material;
        material.SetOverrideTag("RenderType", bugTag);
        material.SetColor("_BugType", (Color32) bugType);
    }

    public void Untag(GameObject go) {
        Material material = go.GetComponent<Renderer>().material;
        material.SetOverrideTag("RenderType", "");
        //material.SetColor("_BugType", (Color32) bugType);
    }
}
