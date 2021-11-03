using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenTearPost : MonoBehaviour {
    
    public bool _enable = true;
    public int n = 1;

    protected float _teary1 = 0.2f;
    protected float _teary2 = 0.3f;
    public Vector2 TearY { 
        get { return new Vector2(_teary1, _teary2); }
        set { 
            _teary1 = value[0];
            _teary2 = value[1]; 
            if (_material != null) {
                _material.SetFloat("_TearMin", _teary1);
                _material.SetFloat("_TearMax", _teary2);
            }
        }
    }

    protected Material _material;
    protected CameraHistory _history;

    void Awake() {
        Shader shader = Shader.Find("Bug/ScreenTear");
        _material = new Material(shader);
        _material.SetFloat("_TearMin", _teary1);
        _material.SetFloat("_TearMax", _teary2);
        _history = gameObject.GetComponent<CameraHistory>();
    }
    
    void OnRenderImage(RenderTexture src, RenderTexture dst) {
        if (_enable && Time.frameCount > 10 + n) {
            _material.SetTexture("_TearTex", _history[n-1]);
            _material.SetTexture("_MainTex", src);
            Graphics.Blit(src, dst, _material);
        } else {
            Graphics.Blit(src, dst);
        }
    }
}
