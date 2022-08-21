using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Policies;

namespace WorldOfBugs {

    public abstract class HeuristicComponent : MonoBehaviour, IHeuristicProvider  {

        public abstract void Heuristic(in ActionBuffers buffer);

        //public void OnEnable() {
        //Debug.Log("ENABLE!");
        // The behaviour type should always be default. The Heuristic method is called by the agent, this is to allow the python process to continue.
        // There may be a more principled way of doing this (like sending a signal back to python somehow, but I can't figure it out... need an ML agents experty here!)
        //GetComponent<BehaviorParameters>().BehaviorType = BehaviorType.Default;
        //}
    }
}
