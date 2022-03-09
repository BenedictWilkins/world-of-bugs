using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Policies;

namespace WorldOfBugs.World1 {

    [AddComponentMenu("World1.Actuator")] 
    public class Actuator : WorldOfBugs.Actuator {
        
        [SerializeField]
        protected string _name;
        public override string Name { get { return _name; }}
        protected readonly ActionSpec _actionSpec = new ActionSpec(0, new int[] { 4 }); 
        public override ActionSpec ActionSpec { get { return _actionSpec; } }
        
        protected delegate void ActionMethod(Agent instance);

        protected static void none(Agent instance) {} // does nothing

        protected  static void forward(Agent instance) { 
            Vector3 movement = instance.gameObject.transform.forward * Time.deltaTime * instance.MovementSpeed;
            instance.gameObject.transform.Translate(movement, Space.World); // TODO change this to move position?
            // Debug.Log($"STEP: {instance.step} FORWARD");
        }

        protected static void rotate_left(Agent instance) {
            instance.gameObject.transform.Rotate(new Vector3(0, -1 * Time.deltaTime * instance.AngularSpeed, 0));
            // Debug.Log($"STEP: {instance.step} ROTATE LEFT");
        }

        protected static void rotate_right(Agent instance) {
            instance.gameObject.transform.Rotate(new Vector3(0, 1 * Time.deltaTime * instance.AngularSpeed, 0));
            // Debug.Log($"STEP: {instance.step} ROTATE RIGHT");
        }

        protected ActionMethod[] actions = new ActionMethod[] { none, forward, rotate_left, rotate_right };

        public override void Execute(ActionBuffers buffer) {
            foreach (int a in buffer.DiscreteActions) {
                actions[a](GetComponent<Agent>()); // perform the action
            }
            //if (.transform.position.y < player.MapBottom) { // fallen out of the map...
            //    EndEpisode();
            //}
        }
    }
}
