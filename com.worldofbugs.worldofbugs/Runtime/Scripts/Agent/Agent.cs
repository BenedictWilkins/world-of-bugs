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
        internal protected HeuristicComponent _heuristic;
        
        [HideInInspector]
        public IReset[] Resets;

        public new void OnEnable() {
            // unfortunately there is no way to create and add an actuator directly (see LazyInitialize/Initialize/InitializeActuators in MLAgents.Agent)
            if (GetComponent<ReflectionActuatorComponent>() == null) {
                ReflectionActuatorComponent actuator = gameObject.AddComponent<ReflectionActuatorComponent>();
                actuator.Initialize(this);
            }
            Resets = GetComponents<IReset>();
            base.OnEnable();
        }

        public void FixedUpdate() {
            foreach (IReset reset in Resets) {
                if (reset.ShouldReset(gameObject)) {
                    EndEpisode();
                }
            }
            RequestDecision();
        }

        public virtual void SetHeuristic(HeuristicComponent heuristic) {
            if (_heuristic != null) {
                _heuristic.enabled = false; _heuristic.enabled = false; 
            }
            _heuristic = heuristic;
            _heuristic.enabled = true;
        }

        public override sealed void Heuristic(in ActionBuffers buffer) {
            if (_heuristic != null) {
                _heuristic.Heuristic(buffer);
            }
        }
            
    }

}