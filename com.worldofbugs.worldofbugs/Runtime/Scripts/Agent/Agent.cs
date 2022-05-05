using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Policies;
using Unity.MLAgents.Sensors.Reflection;

namespace WorldOfBugs { 

    public abstract class Agent : Unity.MLAgents.Agent { 
        
        [SerializeField]
        protected PolicyComponent _policy;
        public PolicyComponent Policy { get { return _policy; } set { _policy = value;}}

        public new void OnEnable() {
            // unfortunately there is no way to create and add an actuator directly (see LazyInitialize/Initialize/InitializeActuators in MLAgents.Agent)
            if (GetComponent<ReflectionActuatorComponent>() == null) {
                ReflectionActuatorComponent actuator = gameObject.AddComponent<ReflectionActuatorComponent>();
                actuator.Initialize(this);
            }
            base.OnEnable();
        }

        public void FixedUpdate() { 
            RequestDecision();
        }

        public override void Heuristic(in ActionBuffers buffer) {
            Debug.Log("Heuristic");
            Policy.Heuristic(buffer);
        }
    }

}