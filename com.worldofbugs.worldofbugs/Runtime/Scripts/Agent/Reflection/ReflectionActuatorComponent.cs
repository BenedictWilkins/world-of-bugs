using System;
using System.Reflection;
using System.Linq;

using UnityEngine;

using Unity.MLAgents;
using Unity.MLAgents.Actuators;

namespace WorldOfBugs {
    
    public class ReflectionActuatorComponent : Unity.MLAgents.Actuators.ActuatorComponent {

        public override ActionSpec ActionSpec { get { return ActionSpec.MakeDiscrete(0); } } // TODO update action spec properly

        private Unity.MLAgents.Agent _agent;
        private IHeuristicProvider _heuristic;

        public void Initialize(Unity.MLAgents.Agent agent) { //stupid unity
            _agent = agent;
        }

        public override IActuator[] CreateActuators() { // initialise must be called before hand...
            Debug.Log("CREATE ACTUATORS");
            IActuator actuator = ActionAttribute.CreateActuator(_agent);
            if (actuator == null) {
                Destroy(_agent.gameObject.GetComponent<ReflectionActuatorComponent>()); // this whole thing is just an annoying work around. Put the code in InitializeActuators in MLAgents package!
                return new IActuator[] {};
            }
            return new IActuator[] { actuator };
        }
    }

    public class ReflectionActuator : IActuator { 

        public string Name { get { return "ReflectionActuator"; }}

        // TODO currently limited to a single branch for discrete actions... why are discrete and continuous actions not treated equally??
    
        public ActionSpec ActionSpec { get { 
            int[] branches = new int[] { _discrete_action_methods.Length };
            if (_discrete_action_methods.Length == 0) { 
                branches = null; 
                }

            return new ActionSpec(_continuous_action_methods.Length, branches); }}
        
        private Action<float>[] _continuous_action_methods;
        private Action[] _discrete_action_methods;
        private IHeuristicProvider _heuristic;

        public ReflectionActuator(IHeuristicProvider heuristic, Action[] discrete_action_methods, Action<float>[] continuous_action_methods) {
            _continuous_action_methods = continuous_action_methods;
            _discrete_action_methods = discrete_action_methods;
            _heuristic = heuristic;
         }

        public void OnActionReceived(ActionBuffers buffer) { // executes discrete actions first, then continuous actions. continuous actions are executed in order of specification 
            // Debug.Log(string.Join(",", buffer.DiscreteActions));
            // Debug.Log($"{buffer.DiscreteActions.Length} {buffer.DiscreteActions[0]}");
            
            // This is the key line that allows a heuristic to be used while in Python, if another actuator is used things will break...
            if (_heuristic != null) {
                Heuristic(buffer); // use the heuristic, it is up to the agent to either replace the action or leave it.
            }

           
            for (int i = 0; i < buffer.DiscreteActions.Length; i++) {
                //Debug.Log($"Discrete Action: {buffer.DiscreteActions[i]}");
                _discrete_action_methods[buffer.DiscreteActions[i]]();
            }
            for (int i = 0; i < buffer.ContinuousActions.Length; i++) {
                //Debug.Log(string.Join(",", buffer.ContinuousActions.Select(x => $"{x}")));
                _continuous_action_methods[i](buffer.ContinuousActions[i]);
            }
        }

        public void Heuristic(in ActionBuffers buffer) {
            _heuristic.Heuristic(buffer);
        }
        
        public void ResetData() {}
        public void WriteDiscreteActionMask(IDiscreteActionMask mask) {}

    }

}
