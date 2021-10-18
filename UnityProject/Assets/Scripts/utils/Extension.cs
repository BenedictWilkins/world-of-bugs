using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

public static class CameraExtensions {

    public static Camera[] GetBugMaskCameras() {
        return Array.FindAll(Camera.allCameras, x => x.GetComponent<BugMaskReplacementShader>() != null).ToArray();
    }
}

