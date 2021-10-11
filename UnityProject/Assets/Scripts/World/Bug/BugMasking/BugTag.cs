using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BugTag : MonoBehaviour
{
    // Start is called before the first frame update
    void OnValidate() {
        //Material material = GetComponent<Renderer>().sharedMaterial;
        //material.SetOverrideTag("RenderType", "Bug");
        //Debug.Log(material);
    }
    
    void Awake() {
        Material material = GetComponent<Renderer>().material;
        material.SetOverrideTag("RenderType", "Bug");
    }
}
