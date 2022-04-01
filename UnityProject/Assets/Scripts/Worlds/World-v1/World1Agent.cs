using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Unity.MLAgents;
using Unity.MLAgents.Sensors.Reflection;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Policies;

namespace WorldOfBugs {

    public class World1Agent : WorldOfBugs.AgentFirstPerson { 

        [Tooltip("Movement speed in units/second")]
        public float MovementSpeed = 2;
        [Tooltip("Angular speed in degrees/second")]
        public float AngularSpeed = 40;
        [Tooltip("Radius of this agent")]
        public float Radius = 1f;

        protected Vector3 initialPosition;
        protected Vector3 initialRotation;

        [Observable("Position")]
        public Vector3 Position;
        [Observable("Rotation")]
        public Vector3 Rotation;
        [Observable("Action")]
        public int Action { 
            get { return GetStoredActionBuffers().DiscreteActions[0]; }
        }

        public void FixedUpdate() { 
            RequestDecision();
        }
       

        public void Awake() {
            initialPosition = gameObject.transform.localPosition;
            initialRotation = gameObject.transform.localEulerAngles;
            gameObject.GetComponent<BehaviorParameters>().ObservableAttributeHandling = ObservableAttributeOptions.ExcludeInherited;
            //UseHeuristic(python); // always start using this heuristic, another can be set in the python API.
        }

        public override void OnEpisodeBegin() {
            gameObject.transform.position = initialPosition;
            //gameObject.transform.localEulerAngles = initialRotation; // new Vector3(0, UnityEngine.Random.value * 360, 0);
        }

        public override void UsePolicy(PolicyComponent policy) {
            Debug.Log($"Using policy: {policy}");
            GetComponent<World1Actuator>().Policy = policy;
        }

    }
}