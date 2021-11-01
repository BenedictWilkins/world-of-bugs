using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Policies;

public class ManualBehaviour : Behaviour {

    public override void Heuristic(in ActionBuffers actionsOut) {
        
        int leftright = (int) Mathf.Round(Input.GetAxis("Horizontal"));
        int forwardback = (int) Mathf.Round(Input.GetAxis("Vertical"));
        
        var _actionsOut = actionsOut.DiscreteActions;
        _actionsOut[0] = 0; //default do nothing

        if (forwardback > 0) {
            _actionsOut[0] = 1; // forward
        } else if (leftright < 0) {
            _actionsOut[0] = 2; // rotate left
        } else if (leftright > 0) {
            _actionsOut[0] = 3; //rotate right
        } 
        //Debug.Log($"MANUAL {_actionsOut[0]}");
    }
}
