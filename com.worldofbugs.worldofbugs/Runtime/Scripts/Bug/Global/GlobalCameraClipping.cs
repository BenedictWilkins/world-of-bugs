using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

using WorldOfBugs;

namespace WorldOfBugs {

    /// <summary>
    /// Sets up the bug mask renderer to render camera clipping issues. This is a non-specific "global" bug, it will happen whenever the associated camera clips through some geometry.
    /// </summary>
    public class GlobalCameraClipping : Bug {

        public static readonly Color TRANSPARENT = new Color(0,0,0,0);

        public override void OnEnable() {
            Camera[] cameras = CameraExtensions.GetObservationCameras();
            // TODO... better to use materials on cameras rather than replacement shader? if the nearclipplan is different for different agents things will go wrong ...
            Shader.SetGlobalFloat("_CameraNearClip", cameras[0].nearClipPlane);
            Shader.SetGlobalColor("_CameraClipColor", (Color)bugType);
        }

        public override void OnDisable() {
            Shader.SetGlobalColor("_CameraClipColor", TRANSPARENT);
        }
    }
}
