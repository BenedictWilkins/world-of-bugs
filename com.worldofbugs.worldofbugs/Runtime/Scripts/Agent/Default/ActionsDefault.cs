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
    public class ActionsDefault {

        // TODO would be nice not to have to add each boolean value, just define a bunch of static methods...? 

        public static void none(GameObject instance) {} // does nothing

        public static void forward(GameObject instance, float movementspeed) {
            move(instance, instance.transform.forward, movementspeed);
        }

        public static void left(GameObject instance, float movementspeed) {
            move(instance, -instance.transform.right, movementspeed);
        }

        public static void right(GameObject instance, float movementspeed) {
            move(instance, instance.transform.right, movementspeed);
        }

        public static void backward(GameObject instance, float movementspeed) {
            move(instance, instance.transform.forward, movementspeed);
        }

        public static void move(GameObject instance, Vector3 direction, float movementspeed) {
            Vector3 movement = direction * Time.deltaTime * movementspeed;
            instance.transform.Translate(movement, Space.World); // TODO change this to move position?
        }

        public static void rotateup(GameObject instance, float angularspeed) {
            instance.transform.Rotate(new Vector3(-1 * Time.deltaTime * angularspeed, 0, 0));
        }

        public static void rotateleft(GameObject instance,  float angularspeed) {
            instance.transform.Rotate(new Vector3(0, -1 * Time.deltaTime * angularspeed, 0));
        }

        public static void rotateright(GameObject instance, float angularspeed) {
            instance.transform.Rotate(new Vector3(0, 1 * Time.deltaTime * angularspeed, 0));
        }

        public static void rotatedown(GameObject instance, float angularspeed) {
            instance.transform.Rotate(new Vector3(1 * Time.deltaTime * angularspeed, 0, 0));
        }

        public static void interact(GameObject instance) {
            //Interactable.Interact(InteractableCollider);
        }

        public static void jump(GameObject instance) {
            // TODO
        }
    }
}