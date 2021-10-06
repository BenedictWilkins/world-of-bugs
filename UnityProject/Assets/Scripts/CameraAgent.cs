using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

public class CameraAgent : Agent
{
    public float rotation_speed = 1f;
    Vector3[] actions = new Vector3[] {
        new Vector3(0f, -1f, 0f),
        new Vector3(0f, 0f, 0f),
        new Vector3(0f, 1f, 0f)
    };

    public override void OnEpisodeBegin() {
        // reset camera position/rotation
        this.transform.localPosition = new Vector3(0f, 0.5f, 0f);
        this.transform.eulerAngles = new Vector3(0f,0f,0f);
    }

    public override void OnActionReceived(ActionBuffers actionBuffers){
        // Actions, size = 2
        Vector3 controlSignal = Vector3.zero;
        int action = 0;

        if (actionBuffers.DiscreteActions.Length > 0) {
            action = actionBuffers.DiscreteActions[0]; // left, nothing, right... ? 
        }
        Debug.Log(action);
        this.transform.eulerAngles += rotation_speed * actions[action];
    }

    public override void Heuristic(in ActionBuffers actionsOut) {
        var actions = actionsOut.DiscreteActions;
        actions[0] = (int) Mathf.Round(Input.GetAxis("Horizontal")) + 1;
    }
}
