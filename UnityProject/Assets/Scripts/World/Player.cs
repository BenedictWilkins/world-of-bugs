using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

public class Player : Agent
{
    public float rotation_speed = 1f;
    public float movement_speed = 0.1f;

    delegate void ActionMethod(Player instance);

     // ACTION METHODS
    static void none(Player instance) {} // does nothing

    static void back(Player instance) {
        instance.transform.position += instance.transform.forward * - instance.movement_speed;
    }

    static void forward(Player instance) { 
        instance.transform.position +=  instance.transform.forward * instance.movement_speed;
    }

    static void left(Player instance) { 
        instance.transform.position += instance.transform.right * - instance.movement_speed;
    }

    static void right(Player instance) {
        instance.transform.position += instance.transform.right * instance.movement_speed;
    }

    static void rotate_left(Player instance) {
        instance.transform.eulerAngles += new Vector3(0f, -instance.rotation_speed, 0f);
    }

    static void rotate_right(Player instance) {
        instance.transform.eulerAngles += new Vector3(0f, instance.rotation_speed, 0f);
    }

    ActionMethod[] actions = {
        none, forward, back, left, right, rotate_left, rotate_right
    };

    public override void OnEpisodeBegin() {
        Vector3 size = this.GetComponent<Collider>().bounds.size;

        // reset position/rotation
        this.transform.localPosition = new Vector3(0f,size.y / 2,0f);
        this.transform.eulerAngles = new Vector3(0f,0f,0f);
    }

    public override void OnActionReceived(ActionBuffers actionBuffers){
        foreach (int action in actionBuffers.DiscreteActions) {
            actions[action](this); // perform the action
        } 

        if (this.transform.localPosition.y < -1) // the agent fell out of the world
        {
            EndEpisode();
        }
    }

    public override void Heuristic(in ActionBuffers actionsOut) {
        int leftright = (int) Mathf.Round(Input.GetAxis("Horizontal"));
        int forwardback = (int) Mathf.Round(Input.GetAxis("Vertical"));
        float rleftright = Input.mousePosition.x;

        var _actionsOut = actionsOut.DiscreteActions;
        _actionsOut[0] = 0; //default do nothing

        if (leftright < 0) { 
            _actionsOut[0] = 3; // left
        } else if (leftright > 0) {
            _actionsOut[0] = 4; // right
        } else if (forwardback > 0) {
            _actionsOut[0] = 1; // forward
        } else if (forwardback < 0) {
            _actionsOut[0] = 2; // back
        } else if (Screen.width - rleftright < 50) {
            _actionsOut[0] = 6; //rotate right
        } else if (rleftright < 50) {
            _actionsOut[0] = 5; // rotate left
        }
        Debug.Log(Screen.width - rleftright);
        Debug.Log(_actionsOut[0]);
    }

   

}
