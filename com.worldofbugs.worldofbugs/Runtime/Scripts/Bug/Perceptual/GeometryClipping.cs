using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
namespace WorldOfBugs {


public class GeometryClipping : Bug {

    [Tooltip("Collection of game objects to select colliders from, only leaf objects will be affected.")]
    public GameObject level;
    [Tooltip("Number of GameObject Colliders to disable, -1 takes all.")]
    public int n; 

    private static System.Random _random = new System.Random();
    private GameObject[] _gos; // disable collider on this game object

    void Awake() {
        // back face bugs (if we can see inside geometry, show it!)
        Shader.SetGlobalColor("_BackFaceColor", (Color)bugType);
    }

    public override void OnEnable() {
        Transform[] children = level.transform.GetComponentsInChildren<Transform>(true);
        children = Array.FindAll(children, x => x.GetComponent<Collider>() != null); // leaf children
        if (n < 0) {
            n = children.Length;
        }
        _gos = children.OrderBy(x => _random.Next()).Take(n).Select(x => x.gameObject).ToArray();
        foreach (GameObject go in _gos) {
            foreach (Collider collider in go.GetComponents<Collider>()) {
                collider.enabled = false;
            }
        }
    }

    public override void OnDisable() {
        if (_gos != null) {
            foreach (GameObject go in _gos) {
                if (go != null) {
                    foreach (Collider collider in go.GetComponents<Collider>()) {
                        collider.enabled = true; // TODO should probably save the state of collider on disable...
                    }
                }   
            }
        }
    }
     

}
}