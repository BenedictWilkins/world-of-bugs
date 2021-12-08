using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GeometryClipping : Bug {

    public GameObject level;
    private GameObject _go; // disable colldier on this game object

    void Awake() {
        // back face bugs (if we can see inside geometry, show it!)
        BugTag tag = GetComponent<BugTag>();
        Shader.SetGlobalColor("_BackFaceColor", (Color)tag.bugType);
    }

    public override void OnEnable() {
        Transform[] children = level.transform.GetComponentsInChildren<Transform>(true);
        children = Array.FindAll(children, x => x.GetComponent<Collider>() != null); // leaf children
        int j = UnityEngine.Random.Range(0, children.Length); 
        _go = children[j].gameObject;
        _go.GetComponent<Collider>().enabled = false;
    }

    public override void OnDisable() {
        if (_go != null) {
            _go.GetComponent<Collider>().enabled = true;
        }
    }
     
    public override bool InView(Camera camera) { 
        return false;
    }
}
