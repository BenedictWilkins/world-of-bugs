using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Policies;

namespace WorldOfBugs {
    public abstract class Actuator : MonoBehaviour, IActuator {

        public abstract string Name { get; }
        public abstract ActionSpec ActionSpec { get; }

        public abstract void Execute(ActionBuffers buffer); // use this one?

        public void OnActionReceived(ActionBuffers buffer) { Execute(buffer); }
        public void ResetData() {}
        public void WriteDiscreteActionMask(IDiscreteActionMask mask) {}
        



    }
}
