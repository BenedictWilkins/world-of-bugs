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

        [SerializeField]
        private Collider _interactableCollider;
        public Collider InteractableCollider {
            get {
                return _interactableCollider;
            }
        }
        [Tooltip("Movement speed in units/second")]
        public float MovementSpeed = 2f;
        [Tooltip("Angular speed in degrees/second")]
        public float AngularSpeed = 40f;
        [Tooltip("Radius of this agent")]
        public float Radius = 1f;

        [Observable("Position")]
        public Vector3 Position {
            get {
                return gameObject.transform.position;
            }
        }
        [Observable("Rotation")]
        public Vector3 Rotation {
            get {
                return gameObject.transform.eulerAngles;
            }
        }
        [Observable("Action")]
        public int Action {
            get {
                return GetStoredActionBuffers().DiscreteActions[0];
            }
        }

        public string[] ActionMeanings {
            get {
                return ActionAttribute.ActionMeanings(this);
            }
        }

        protected Vector3 initialPosition;
        protected Vector3 initialRotation;

        public void Awake() {
            initialPosition = gameObject.transform.localPosition;
            initialRotation = gameObject.transform.localEulerAngles;

            if(_heuristic != null) {
                _heuristic.enabled = true;
            }
        }

        public override void Reset() {
            gameObject.transform.position = initialPosition;
        }
    }
}
