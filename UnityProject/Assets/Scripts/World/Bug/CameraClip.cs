using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CameraClip : Bug {

    public Camera _camera;
    public float clip;
    protected float _oldclip;
    protected Color transparent = new Color(0,0,0,0);
    
    void Awake() {
        if (_camera == null) {
            Camera[] mainCameras = Array.FindAll(Camera.allCameras, x => x.tag == "MainCamera").ToArray(); 
            if (mainCameras.Length > 1) {
                Debug.LogWarning("Found more than one main camera, CameraClip bug may not work as expected.");
            }   
            _camera = mainCameras[0];
        } 
    
        OnDisable();
    }

    public void OnEnable() {
        // set the near clipping plan to be farther away
        // when close to an object the view will clip inside
        _oldclip = _camera.nearClipPlane;
        _camera.nearClipPlane = clip;
        BugTag tag = GetComponent<BugTag>();
        Shader.SetGlobalFloat("_CameraNearClip", clip);
        Shader.SetGlobalColor("_CameraClipColor", (Color)tag.bugType);
    }

    public void OnDisable() {
        Shader.SetGlobalFloat("_CameraNearClip", 0.01f);
        Shader.SetGlobalColor("_CameraClipColor", transparent);
    }

    void OnDestroy() {
        OnDisable();
    }

    public override bool InView(Camera camera) { 
        if (gameObject.activeSelf) {
            BugTag tag = GetComponent<BugTag>(); 
            int[] mask = BugMask.Instance.Mask(camera); 
            // Compare the mask with my bug type...
            bool result = mask.Contains((int) tag.bugType);
            return result;
        }
        return false;
    }



}
