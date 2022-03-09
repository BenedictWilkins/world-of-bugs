using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Policies;

namespace WorldOfBugs { 
    public class HeuristicIdentity : Heuristic {
        public override void Decide(in ActionBuffers buffer) {
            //Debug.Log("HEURISTIC!");
        }
    }
}
