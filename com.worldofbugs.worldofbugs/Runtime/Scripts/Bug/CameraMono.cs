using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMono : MonoBehaviour {

    public Color color;
    protected Material _material;
    public bool mono = false;

    void OnRenderImage(RenderTexture src, RenderTexture dst) {
        if(mono) {
            Graphics.Blit(src, dst, _material);
        } else {
            Graphics.Blit(src, dst);
        }
    }

    void Awake() {
        Shader shader = Shader.Find("Bug/Mono");
        _material = new Material(shader);
    }

    void Start() {
        _material.SetColor("_BugType", (Color32) color);
    }
}
