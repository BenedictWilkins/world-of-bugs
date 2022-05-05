using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

using WorldOfBugs;

public class CameraClipping : Bug {

    public float clip;

    [SerializeField]
    protected Camera mainCamera;
    protected float _oldclip;
    protected Color transparent = new Color(0,0,0,0);
    
    void Awake() {
        Debug.Log(mainCamera.nearClipPlane);
        _oldclip = mainCamera.nearClipPlane;
        OnDisable();
    }

    public override void OnEnable() {
        // set the near clipping plan to be farther away
        // when close to an object the view will clip inside
       
        mainCamera.nearClipPlane = clip;
        Shader.SetGlobalFloat("_CameraNearClip", clip);
        Shader.SetGlobalColor("_CameraClipColor", (Color)bugType);
    }

    public override void OnDisable() {
        if (mainCamera != null) {
            mainCamera.nearClipPlane = _oldclip;
        }
        Shader.SetGlobalFloat("_CameraNearClip", _oldclip);
        Shader.SetGlobalColor("_CameraClipColor", transparent);
    }

    void OnDestroy() {
        OnDisable();
    }
}
