using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BugTag : MonoBehaviour {  

    [SerializeField]
    public BugType bugType; 

    void Awake() {
        Material material = GetComponent<Renderer>().material;
        material.SetOverrideTag("RenderType", "Bug");
        material.SetColor("_BugType", (Color32) bugType);
    }
}
