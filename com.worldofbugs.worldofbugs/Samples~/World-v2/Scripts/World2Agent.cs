using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Unity.MLAgents;
using Unity.MLAgents.Sensors.Reflection;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Policies;

namespace WorldOfBugs {

    public class World2Agent : WorldOfBugs.Agent {

        public float MovementSpeed = 1.0f;
        
        [Action]
        public void none() {} // does nothing

        [Action]
        public void forward() {
            move(gameObject.transform.forward);
        }

        [Action]
        public void face(float angle) {
            
        }

        public void move(Vector3 direction) {
            Vector3 movement = direction * Time.deltaTime * MovementSpeed;
            gameObject.transform.Translate(movement, Space.World); // TODO change this to move position?
        }

        public override void OnActionReceived(ActionBuffers actions) {
            Debug.Log("????");
        }

        public override void Heuristic(in ActionBuffers buffer) {
            Debug.Log("?");
        }



    }
}