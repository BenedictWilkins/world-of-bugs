using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

using WorldOfBugs;

namespace WorldOfBugs {

    public class GlobalBackside : Bug {

        public static readonly Color TRANSPARENT = new Color(0,0,0,0);

        public override void OnEnable() {
            Camera[] cameras = CameraExtensions.GetObservationCameras();
            Shader.SetGlobalColor("_BackFaceColor", (Color)bugType);
        }

        public override void OnDisable() {
            Shader.SetGlobalColor("_BackFaceColor", TRANSPARENT);
        }
    }
}
