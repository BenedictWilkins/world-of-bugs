using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Rendering;

public class GeometryCorruption : Bug {

    [Range(0f,10f)]
    public float power = 10f;
    public bool _static = true;
    public GameObject level;

    protected Vector3[] vertices;
    protected ShadowCastingMode shadowMode;
    protected GameObject _go;

    public override void OnEnable() {
        // find a suitable game object...
        Transform[] children = level.transform.GetComponentsInChildren<Transform>(true);
        children = Array.FindAll(children, x => x.GetComponent<Renderer>() != null); // leaf children
        int j = UnityEngine.Random.Range(0, children.Length); 
        _go = children[j].gameObject;
        Mesh mesh = _go.GetComponent<MeshFilter>().mesh;
        vertices = mesh.vertices;
        
        Renderer renderer = _go.GetComponent<Renderer>();
        shadowMode = renderer.shadowCastingMode;
        renderer.shadowCastingMode = ShadowCastingMode.Off;

        Corrupt();
        Tag(_go);
    }

    public override void OnDisable() {
       Reset();
    }

    void Update() {
        if (!_static) {
            Corrupt();
        }
    }

    public void Corrupt() {
        Mesh mesh = _go.GetComponent<MeshFilter>().mesh;
        Vector3[] verts = mesh.vertices;
        Vector3[] normals = mesh.normals;
        for (var i = 0; i < vertices.Length; i++){
            verts[i] += normals[i] * (2 * UnityEngine.Random.value - 1)  * power;
            //verts[i] += RandomVector3() * power;
        }
        mesh.vertices = verts;
    }

    public void Reset() {
        if (_go != null) {
            Mesh mesh = _go.GetComponent<MeshFilter>().mesh;
            mesh.vertices = vertices; // reset the verticies...
           
            Untag(_go);
            Renderer renderer = _go.GetComponent<Renderer>();
            renderer.shadowCastingMode = shadowMode;
        }
    }

    public Vector3 RandomVector3() {
        return 2f * new Vector3(UnityEngine.Random.value, UnityEngine.Random.value,UnityEngine.Random.value) - Vector3.one; 
    }
}
