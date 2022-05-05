using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;


using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Policies;

namespace WorldOfBugs {

    public class ActuatorDefault : ActuatorComponent<AgentDefault> {
       
        public override IActuator[] CreateActuators() {
            return new IActuator[] { new _ActuatorDefault(this) };
        }
    }
    
    public class _ActuatorDefault : Actuator<AgentDefault> {
        
        protected Action<AgentDefault>[] _actions; // = new Action<AgentDefault>[] { none, forward, rotate_left, rotate_right };

        public _ActuatorDefault(ActuatorDefault component) : base(component) { 
            _actions = component.Actions.ActionDelegates(); 
        }

        public override void OnActionReceived(ActionBuffers buffer) {
            int _a = buffer.DiscreteActions[0];
            if (_policy.isHeuristic) {
                Heuristic(buffer);
            }
            int action = buffer.DiscreteActions[0];
            // Debug.Log($"{_a} {action}");
            _actions[action](_component.gameObject.GetComponent<AgentDefault>()); // perform the action
        }

        public override void Heuristic(in ActionBuffers buffer) {
            //Debug.Log("USE HEURISTIC");
            _policy.Heuristic(buffer);
        }

        public override void ResetData() {}
        public override void WriteDiscreteActionMask(IDiscreteActionMask mask) {}

    }
}
