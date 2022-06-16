using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace WorldOfBugs {


public class TextureCorrupt : Bug {

    public GameObject level;
    [Range(0.05f,1f)]
    public float uvcomplexity = 0.2f;

    protected GameObject _go;
    protected Vector2 _textureOffset;
    protected Vector2[] _uv;

    public override void OnEnable() {
        // get children of the given game object (level)
        Transform[] children = level.transform.GetComponentsInChildren<Transform>(true);
        children = Array.FindAll(children, x => x.GetComponent<Renderer>() != null); // leaf children
        int i = UnityEngine.Random.Range(0, children.Length);
        _go = children[i].gameObject;
        Material material = _go.GetComponent<Renderer>().material;
        Mesh mesh = _go.GetComponent<MeshFilter>().mesh;
        _textureOffset = material.mainTextureOffset;
        _uv = mesh.uv;
        Vector3 size  = mesh.bounds.size;
        float m = Mathf.Max(Mathf.Max(size.x, size.y), size.z);
        material.mainTextureOffset = new Vector2(_GetRandom() * m, _GetRandom() * m);
        mesh.uv = RandomUV(mesh);
        Tag(_go);
    }

    private float _GetRandom() {
        return Mathf.Min(UnityEngine.Random.value + 0.2f, 1f);
    }

    private Vector2[] RandomUV(Mesh mesh) {
        Vector2[] uvs = mesh.uv;
        Vector2[] _uvs = new Vector2[uvs.Length];
        bool modified = false;
        for (int i = 0; i < uvs.Length; i++) {
            if (UnityEngine.Random.value < uvcomplexity) {
                _uvs[i] = new Vector2(UnityEngine.Random.value, UnityEngine.Random.value);
                modified = true;
            } else {
                _uvs[i] = uvs[i];
            }
        }
        if (!modified) { // if the uv map wasnt modified, it should be, this is a bug after all!
            int i = UnityEngine.Random.Range(0, uvs.Length);
            _uvs[i] = new Vector2(UnityEngine.Random.value, UnityEngine.Random.value);
        }
        return _uvs;
    }

    public override void OnDisable() {
        if (_go != null) {
            Material material = _go.GetComponent<Renderer>().material;
            Mesh mesh = _go.GetComponent<MeshFilter>().mesh;
            material.mainTextureOffset = _textureOffset;
            mesh.uv = _uv;

            Untag(_go);
            _go = null;
        }
    }
}
}
