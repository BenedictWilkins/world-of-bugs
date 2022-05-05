using System.Collections;
using System.Collections.Generic;
using UnityEngine;


using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Policies;
using Unity.MLAgents.Sensors.Reflection;

namespace WorldOfBugs { 

    public abstract class AgentFirstPerson : Agent {

        [SerializeField, NotNull, Tooltip("Camera that is used to create a view of the world.")]
        private Camera _cameraMain;
        [SerializeField, NotNull, Tooltip("Camera that is used to create the bug mask, this camera should use the ShowBug shader and a RenderTexture.")]
        private Camera _cameraBugMask; // camera used to detect bugs
        
        public Camera CameraBugMask { get { return _cameraBugMask; }}
        public Camera CameraMain { get { return _cameraMain; }}
        
    }
}