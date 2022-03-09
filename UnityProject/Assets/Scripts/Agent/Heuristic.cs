using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Policies;

namespace WorldOfBugs { 
    public abstract class Heuristic : MonoBehaviour {
        public abstract void Decide(in ActionBuffers buffer); 
    }
}
