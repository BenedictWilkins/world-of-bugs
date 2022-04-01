using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Policies;

namespace WorldOfBugs { 

    public class PolicyPython : PolicyComponent  {

        public override bool isHeuristic { get { return false; }}
        public override void Heuristic(in ActionBuffers buffer) { 
            throw new WorldOfBugsException("This heuristic is meant as a placeholder for a python policy, something went wrong.");
        }
    }
}
