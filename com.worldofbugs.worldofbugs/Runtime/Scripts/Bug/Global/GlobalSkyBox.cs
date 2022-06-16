using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


namespace WorldOfBugs {

    /// <summary>
    /// Sets up the bug mask renderer to render camera clipping issues. This is a non-specific "global" bug, it will happen whenever the associated camera clips through some geometry.
    /// </summary>
    public class GlobalSkyBox : Bug {


        // TODO similar thing are done else where, just use reflection for it?
        private struct CameraProperties {
            Camera camera;
            Color backgroundColor;
            CameraClearFlags clearFlags;
            public CameraProperties(Camera _camera) {
                camera = _camera;
                backgroundColor = _camera.backgroundColor;
                clearFlags = _camera.clearFlags;
            }

            public void Reset() {
                camera.backgroundColor = backgroundColor;
                camera.clearFlags = clearFlags;
            }
        }

        private List<CameraProperties> ModifiedCameras;

        public override void OnEnable() {
            ModifiedCameras = new List<CameraProperties>();
            Camera[] cameras = CameraExtensions.GetBugMaskCamera();
            foreach (Camera camera in cameras) {
                ModifiedCameras.Add(new CameraProperties(camera));
                camera.backgroundColor = bugType;
                camera.clearFlags = CameraClearFlags.SolidColor;
            }
        }

        public override void OnDisable() {
            foreach (CameraProperties properties in ModifiedCameras) {
                properties.Reset();
            }
        }
    }
}
