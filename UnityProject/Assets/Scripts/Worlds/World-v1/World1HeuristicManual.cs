using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Policies;

namespace WorldOfBugs {
    
    public class World1HeuristicManual : HeuristicComponent {

        public override void Heuristic(in ActionBuffers buffer) {
            int leftright = (int) Mathf.Round(Input.GetAxis("Horizontal"));
            int forwardback = (int) Mathf.Round(Input.GetAxis("Vertical"));
            var _buffer = buffer.DiscreteActions;
            _buffer[0] = 0; //default do nothing
            if (forwardback > 0) {
                _buffer[0] = 1; // forward
            } else if (leftright < 0) {
                _buffer[0] = 2; // rotate left
            } else if (leftright > 0) {
                _buffer[0] = 3; //rotate right
            } 
        }
        
    }
}
