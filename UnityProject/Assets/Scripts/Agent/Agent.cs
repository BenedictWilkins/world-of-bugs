using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Policies;

namespace WorldOfBugs { 

    public abstract class Agent : Unity.MLAgents.Agent { 
        
        public IActuator[] Actuators { get { return gameObject.GetComponents<IActuator>(); } }
        
        public ISensor[] Sensors { get { return gameObject.GetComponents<ISensor>(); }}

        [SerializeField]
        protected Heuristic heuristic;

        public override void Heuristic(in ActionBuffers buffer) {
            heuristic.Decide(buffer);
        }

        public override void OnActionReceived(ActionBuffers buffer) {
            Actuators[0].OnActionReceived(buffer); // TODO sure we can deal with branches etc. This might be for the future, there is not good support for this in MLAgents...
        }
    }
}