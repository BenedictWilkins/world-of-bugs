using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Policies;

namespace WorldOfBugs { 
    public abstract class HeuristicComponent : MonoBehaviour, IHeuristicProvider  {
        public abstract void Heuristic(in ActionBuffers buffer);
    }
}
