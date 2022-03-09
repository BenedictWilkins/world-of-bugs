using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Policies;

namespace WorldOfBugs.World1 {

    [AddComponentMenu("World1.Agent")] 
    public class Agent : WorldOfBugs.AgentFirstPerson { 

        [Tooltip("Movement speed in units/second")]
        public float MovementSpeed = 2;
        [Tooltip("Angular speed in degrees/second")]
        public float AngularSpeed = 40;
        [Tooltip("Radius of this agent")]
        public float Radius = 0.5f;

        protected Vector3 initialPosition;
        protected Vector3 initialRotation;

        public void Awake() {
            initialPosition = gameObject.transform.localPosition;
            initialRotation = gameObject.transform.localEulerAngles;
        }

        public void OnEpisodeBegin() {
            gameObject.transform.position = initialPosition;
            //gameObject.transform.localEulerAngles = initialRotation; // new Vector3(0, UnityEngine.Random.value * 360, 0);
        }

    }
}