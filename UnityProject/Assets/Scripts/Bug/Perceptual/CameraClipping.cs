using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

using WorldOfBugs;

public class CameraClipping : Bug {

    public AgentFirstPerson player;
    public float clip;
    protected Camera _camera;
    protected float _oldclip;
    protected Color transparent = new Color(0,0,0,0);
    
    void Awake() {
        _camera = player.CameraMain;
        Debug.Log(_camera.nearClipPlane);
        _oldclip = _camera.nearClipPlane;
        OnDisable();
    }

    public override void OnEnable() {
        // set the near clipping plan to be farther away
        // when close to an object the view will clip inside
       
        _camera.nearClipPlane = clip;
        Shader.SetGlobalFloat("_CameraNearClip", clip);
        Shader.SetGlobalColor("_CameraClipColor", (Color)bugType);
    }

    public override void OnDisable() {
        if (_camera != null) {
            _camera.nearClipPlane = _oldclip;
        }
        Shader.SetGlobalFloat("_CameraNearClip", _oldclip);
        Shader.SetGlobalColor("_CameraClipColor", transparent);
    }

    void OnDestroy() {
        OnDisable();
    }
}
