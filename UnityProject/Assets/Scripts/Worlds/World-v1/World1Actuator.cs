using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Policies;

namespace WorldOfBugs {

    public class World1Actuator : ActuatorComponent {
       
        public string Name; 
        public PolicyComponent Policy;

        public override ActionSpec ActionSpec { get { return ActionSpec.MakeDiscrete(4); } }

        public override IActuator[] CreateActuators(){
            return new IActuator[] { new _World1Actuator(this) };
        }
    }

    public class _World1Actuator : IActuator {
        
        private World1Actuator _component;
        private PolicyComponent _policy { get { return _component.Policy; }}

        public string Name { get { return _component.Name; }}
        public ActionSpec ActionSpec { get { return _component.ActionSpec; } }
        
        public _World1Actuator(World1Actuator component) { _component = component; }
        
        protected delegate void ActionMethod(World1Agent instance);

        protected static void none(World1Agent instance) {} // does nothing

        protected  static void forward(World1Agent instance) { 
            Vector3 movement = instance.gameObject.transform.forward * Time.deltaTime * instance.MovementSpeed;
            instance.gameObject.transform.Translate(movement, Space.World); // TODO change this to move position?
            // Debug.Log($"STEP: {instance.step} FORWARD");
        }

        protected static void rotate_left(World1Agent instance) {
            instance.gameObject.transform.Rotate(new Vector3(0, -1 * Time.deltaTime * instance.AngularSpeed, 0));
            // Debug.Log($"STEP: {instance.step} ROTATE LEFT");
        }

        protected static void rotate_right(World1Agent instance) {
            instance.gameObject.transform.Rotate(new Vector3(0, 1 * Time.deltaTime * instance.AngularSpeed, 0));
            // Debug.Log($"STEP: {instance.step} ROTATE RIGHT");
        }

        protected ActionMethod[] actions = new ActionMethod[] { none, forward, rotate_left, rotate_right };

        public void OnActionReceived(ActionBuffers buffer) {
            int _a = buffer.DiscreteActions[0];
            if (_policy.isHeuristic) {
                Heuristic(buffer);
            }
            int action = buffer.DiscreteActions[0];
            Debug.Log($"{_a} {action}");
            actions[action](_component.GetComponent<World1Agent>()); // perform the action
        }

        public void Heuristic(in ActionBuffers buffer) {
            //Debug.Log("USE HEURISTIC");
            _policy.Heuristic(buffer);
        }

        public void ResetData() {}
        public void WriteDiscreteActionMask(IDiscreteActionMask mask) {}
    }
}
