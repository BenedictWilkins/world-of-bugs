using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Policies;

public class Behaviour : Agent {

    protected Player player;

    protected int movementDirection;
    protected int rotationDirection;

    protected int action = 0; // the action that is currently being taken
    //protected bool done = false; // where the episode ended now.

    public Player Player { get {return player;}} 
    public int Action { get {return action;}}
    //public bool Done { get {return done;}}

    protected int step = 0;

    void Awake() {
        player = transform.parent.gameObject.GetComponent<Player>();
    }

    protected delegate void ActionMethod(Behaviour instance);

     // ACTION METHODS
    protected  static void none(Behaviour instance) {
        instance.movementDirection = 0;
        instance.rotationDirection = 0;
        Debug.Log($"STEP: {instance.step} NOTHING");
    } // does nothing


    protected  static void forward(Behaviour instance) { 
        instance.movementDirection = 1;
        instance.rotationDirection = 0;
        Debug.Log($"STEP: {instance.step} FORWARD");
    }

    protected  static void rotate_left(Behaviour instance) {
        instance.rotationDirection = -1;
        instance.movementDirection = 0;
        Debug.Log($"STEP: {instance.step} ROTATE LEFT");
    }

    protected static void rotate_right(Behaviour instance) {
        instance.rotationDirection = 1;
        instance.movementDirection = 0;
        Debug.Log($"STEP: {instance.step} ROTATE RIGHT");
    }

    protected ActionMethod[] actions = {
        // ACTION LABELS
        none, forward, rotate_left, rotate_right
    };

    public override void OnActionReceived(ActionBuffers actionBuffers){
        foreach (int a in actionBuffers.DiscreteActions) {
            actions[a](this); // perform the action
            action = a; // save the action to be part of the observation meta data
        }
        step += 1; 

        if (player.transform.position.y < player.MapBottom) { // fallen out of the map...
            EndEpisode();
        }

        ExecuteAction();
    }

    public override void OnEpisodeBegin() {
        //Debug.Log("EPISODE BEGIN!");
        player.OnEpisodeBegin();
        step = 0;
    }

    public void ExecuteAction() {
        Debug.Log($"STEP: {step} FIXED UPDATE {rotationDirection} {movementDirection}");
        // based on the agents selected actions, move/rotate etc...
        if (rotationDirection != 0) {
            Vector3 rotate = new Vector3(0, rotationDirection * Time.deltaTime * player.angularSpeed, 0);
            player.transform.Rotate(rotate);
        }
        if (movementDirection != 0) {
            Vector3 movement = player.transform.forward * Time.deltaTime * movementDirection * player.movementSpeed;
            player.transform.Translate(movement, Space.World); // change this to move position?
        }

        
    }
}
