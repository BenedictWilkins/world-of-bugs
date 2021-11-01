using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenTearPost : MonoBehaviour {

    protected Material _material;
    public bool _enable = true;

    void OnRenderImage(RenderTexture src, RenderTexture dst) {
        if (_enable) {
            _material.SetTexture("_MainTex", src);
            Graphics.Blit(src, dst, _material);
        } else {
            Graphics.Blit(src, dst);
        }
    }

    void Awake() {
        Shader shader = Shader.Find("Bug/ScreenTear");
        _material = new Material(shader);
    }
}
