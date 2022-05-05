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

    [Serializable]
    public class ActionsDefault : Actions<AgentDefault> {

        // TODO would be nice not to have to add each boolean value, just define a bunch of static methods...? 

        public bool None = false;
        public bool Forward = false;
        public bool Left = false;
        public bool Right = false;
        public bool Backward = false;

        public bool RotateUp = false;
        public bool RotateLeft = false;
        public bool RotateRight = false;
        public bool RotateDown = false;

        public bool Interact = false;
        public bool Jump = false;

        public static void none(AgentDefault instance) {} // does nothing

        public static void forward(AgentDefault instance) {
            move(instance, instance.gameObject.transform.forward);
        }

        public static void left(AgentDefault instance) {
            move(instance, -instance.gameObject.transform.right);
        }

        public static void right(AgentDefault instance) {
            move(instance, instance.gameObject.transform.right);
        }

        public static void backward(AgentDefault instance) {
            move(instance, -instance.gameObject.transform.forward);
        }

        public static void move(AgentDefault instance, Vector3 direction) {
            Vector3 movement = direction * Time.deltaTime * instance.MovementSpeed;
            instance.gameObject.transform.Translate(movement, Space.World); // TODO change this to move position?
        }

        public static void rotateup(AgentDefault instance) {
            instance.gameObject.transform.Rotate(new Vector3(-1 * Time.deltaTime * instance.AngularSpeed, 0, 0));
        }

        public static void rotateleft(AgentDefault instance) {
            instance.gameObject.transform.Rotate(new Vector3(0, -1 * Time.deltaTime * instance.AngularSpeed, 0));
        }

        public static void rotateright(AgentDefault instance) {
            instance.gameObject.transform.Rotate(new Vector3(0, 1 * Time.deltaTime * instance.AngularSpeed, 0));
        }

        public static void rotatedown(AgentDefault instance) {
            instance.gameObject.transform.Rotate(new Vector3(1 * Time.deltaTime * instance.AngularSpeed, 0, 0));
        }

        public static void interact(AgentDefault instance) {
            Interactable.Interact(instance.InteractableCollider);
        }

        public static void jump(AgentDefault instance) {
            // TODO
        }
    }
}