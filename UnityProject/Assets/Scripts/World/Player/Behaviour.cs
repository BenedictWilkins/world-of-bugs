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

    void Awake() {
        player = transform.parent.gameObject.GetComponent<Player>();
    }

    protected delegate void ActionMethod(Behaviour instance);

     // ACTION METHODS
    protected  static void none(Behaviour instance) {
        instance.movementDirection = 0;
        instance.rotationDirection = 0;
    } // does nothing


    protected  static void forward(Behaviour instance) { 
        instance.movementDirection = 1;
        instance.rotationDirection = 0;
    }

    protected  static void rotate_left(Behaviour instance) {
        instance.rotationDirection = -1;
        instance.movementDirection = 0;
    }

    protected  static void rotate_right(Behaviour instance) {
        instance.rotationDirection = 1;
        instance.movementDirection = 0;
    }

    protected ActionMethod[] actions = {
        none, forward, rotate_left, rotate_right
    };

    public override void OnActionReceived(ActionBuffers actionBuffers){
        foreach (int a in actionBuffers.DiscreteActions) {
            actions[a](this); // perform the action
            action = a; // save the action to be part of the observation meta data
        } 
    }

    public override void OnEpisodeBegin() {
        //Debug.Log("EPISODE BEGIN!");
        player.OnEpisodeBegin();
    }

    public void FixedUpdate() {
        // based on the agents selected actions, move/rotate etc...
        if (rotationDirection != 0) {
            Vector3 rotate = new Vector3(0, rotationDirection * Time.deltaTime * player.angularSpeed, 0);
            player.transform.Rotate(rotate);
        }
        if (movementDirection != 0) {
            Vector3 movement = player.transform.forward * Time.deltaTime * movementDirection * player.movementSpeed;
            player.transform.Translate(movement, Space.World); // change this to move position?
        }

        if (player.transform.position.y < player.MapBottom) { // fallen out of the map...
            EndEpisode();
        }
    }
}
