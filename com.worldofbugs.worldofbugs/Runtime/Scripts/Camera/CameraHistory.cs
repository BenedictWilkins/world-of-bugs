using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraHistory : MonoBehaviour {

    public float delta = 0.01f;

    private int _n = 1;
    private Camera _camera;
    private RenderTexture[] _previousTextures; //circular buffer
    private int _ptindex = 0;
    private float _ptime;

    public int Length => _previousTextures.Length; // should reflect _n

    public int n {
        get {
            return _n;
        }
        set {
            _n = Mathf.Max(_n, value);

            if(_n > Length) {
                RenderTexture[] _pt = new RenderTexture[_n];

                for(int i = 0; i < _previousTextures.Length; i++) {
                    _pt[i] = _previousTextures[i];
                }

                for(int i = _previousTextures.Length; i < _pt.Length; i++) {
                    _pt[i] = new RenderTexture(_camera.targetTexture);
                }

                _previousTextures = _pt;
            }
        }
    }

    public RenderTexture this[int index] {
        get {
            //Debug.Log($"{n} {index} {(n + _ptindex - index) % n}");
            if(index >= n) {
                throw new IndexOutOfRangeException();
            }

            return _previousTextures[(n + _ptindex - index) % n];
        }
    }

    void Awake() {
        _ptime = Time.time;
        _camera = GetComponent<Camera>();
        _previousTextures = new RenderTexture[n];

        for(int i = 0; i < _previousTextures.Length; i++) {
            _previousTextures[i] = new RenderTexture(_camera.targetTexture);
        }
    }

    void OnRenderImage(RenderTexture src, RenderTexture dst) {
        Graphics.Blit(src, dst);

        if(Time.time - _ptime > delta) {
            _ptime = Time.time;
            _ptindex = (_ptindex +  1) % n;
            // save this frame for use in the history, note that this history is dependant on delta, but is independant of the application framerate.
            Graphics.Blit(src, _previousTextures[_ptindex]);
            //Debug.Log($"{_previousTextures[_ptindex]} {Time.time}");
        }
    }

}
