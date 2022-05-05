using System.Collections;
using System.Collections.Generic;
using UnityEngine;


using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Policies;
using Unity.MLAgents.Sensors.Reflection;

namespace WorldOfBugs { 

    public abstract class AgentDefault : AgentFirstPerson {
        
        public static readonly int RESET_Y = -5;
        
        [SerializeField]
        private Collider _interactableCollider;
        public Collider InteractableCollider { get { return _interactableCollider; } }
        
        [Tooltip("Movement speed in units/second")]
        public float MovementSpeed = 2f;
        [Tooltip("Angular speed in degrees/second")]
        public float AngularSpeed = 40f;
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

        public new void FixedUpdate() { 
            RequestDecision();
            if (transform.position.y < RESET_Y) {
                EndEpisode();
            }
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
    }
}