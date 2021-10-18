using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlackScreen : Bug {
    
    public Camera _camera; 
    public Vector2Int range = new Vector2Int(5, 50);

    protected bool _enabled = false;
    protected int lastFrame = 0;
    protected int toWait = 0;

    protected delegate void OnOff();
    protected OnOff[] _OnOff;
    protected int _toggle;

    protected CameraMono[] monos;

    protected void On() {
        _camera.SetReplacementShader(Shader.Find("Bug/Empty"), "RenderType");
        foreach(CameraMono mono in monos) {
            mono.mono = true;
        }
    }

    protected void Off() {
        if (_camera != null) {
            _camera.ResetReplacementShader();
        }
        foreach(CameraMono mono in monos) {
            if (mono != null) {
                mono.mono = false;
            }
        }
    }

    void Awake() { 
        // TODO make this work for multiple main cameras aswell? 
        // CameraMono could also be used for create the black screen (instead of disabling the shader?) who knows...
        _OnOff = new OnOff[] { this.On, this.Off };
        Camera[] cameras = CameraExtensions.GetBugMaskCameras();
        monos = new CameraMono[cameras.Length];
        for (int i = 0; i < cameras.Length; i++) {
            CameraMono mono = cameras[i].gameObject.AddComponent<CameraMono>();
            mono.color = (Color) GetComponent<BugTag>().bugType;
            monos[i] = mono;
        }
    }

    public void OnEnable() {
        _enabled = true;
    }

    public void OnDisable() {
        _enabled = false;
    }

    void toggle() {
        _toggle = 1 - _toggle;
        _OnOff[_toggle]();
    }
    
    void OnDestroy() {
        Off();
    }

    public override bool InView(Camera camera) { 
        return false;
    }

    void Update() { 
        if (_enabled) {
            if (Time.frameCount - lastFrame > toWait) {
                toWait = UnityEngine.Random.Range(range.x, range.y);
                lastFrame = Time.frameCount;
                toggle();
            }
        }
    }
}
