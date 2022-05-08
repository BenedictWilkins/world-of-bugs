using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Policies;

namespace WorldOfBugs { 
    
    public class HeuristicIdentity : HeuristicComponent {

        public override void Heuristic(in ActionBuffers buffer) {}
        
    }
}
