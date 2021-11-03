using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

public static class CameraExtensions {

    public static Camera[] GetBugMaskCameras() {
        return Array.FindAll(Camera.allCameras, x => x.GetComponent<BugMaskReplacementShader>() != null).ToArray();
    }

    public static Camera[] GetCamerasByRenderTexture(RenderTexture texture) {
        Camera[] cameras = Array.FindAll(Camera.allCameras, x => x.targetTexture == texture);
        Array.Sort<Camera>(cameras, (x,y) => - x.depth.CompareTo(y.depth)); // sort by depth, the camera that renders last is first
        return cameras.ToArray();
    }
}



