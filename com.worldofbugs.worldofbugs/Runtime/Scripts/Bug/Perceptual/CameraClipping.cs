using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

using WorldOfBugs;

public class CameraClipping : Bug {
    
    [Tooltip("Set the new near clipping plane for all observation cameras, this may induce clipping issues.")]
    public float NearClipPlane;
    
    protected Camera[] Cameras;
    protected float[] OldNearClipPlane;
   
    public override void OnEnable() {
        // set the near clipping plan to be farther away
        // when close to an object the view will clip inside
        Cameras = CameraExtensions.GetObservationCameras();
        OldNearClipPlane = Cameras.Select(x => x.nearClipPlane).ToArray();
        foreach (Camera camera in Cameras) {
            camera.nearClipPlane = NearClipPlane;
        }
        // TODO set camera material _CameraNearClip rather than global? 
        Shader.SetGlobalFloat("_CameraNearClip", NearClipPlane);
    }

    public override void OnDisable() {
        for (int i = 0; i < Cameras.Length; i++) {
            Cameras[i].nearClipPlane = OldNearClipPlane[i];
        }
        // TODO on per camera basis with materials...
        Shader.SetGlobalFloat("_CameraNearClip", OldNearClipPlane[0]);
    }
}
