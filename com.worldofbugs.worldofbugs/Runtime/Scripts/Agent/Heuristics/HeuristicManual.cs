using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

using UnityEngine;

using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Policies;

namespace WorldOfBugs {

    public class HeuristicManual : HeuristicComponent {

        protected static string[] REQUIRED_ACTION_MEANINGS = new string[] { "none", "forward", "rotateleft", "rotateright" };
        protected string[] actionmeanings;

        public override void Heuristic(in ActionBuffers buffer) {
            int leftright = (int) Mathf.Round(Input.GetAxis("Horizontal"));
            int forwardback = (int) Mathf.Round(Input.GetAxis("Vertical"));
            //bool interact =  Input.GetKeyDown("space");
            var _buffer = buffer.DiscreteActions;
            _buffer[0] = 0; // default do nothing

            if(forwardback > 0) {
                _buffer[0] = 1; // forward
            } else if(leftright < 0) {
                _buffer[0] = 2; // rotate left
            } else if(leftright > 0) {
                _buffer[0] = 3; //rotate right
            }
        }

        public void OnEnable() {
            string[] am = ActionAttribute.ActionMeanings(GetComponent<Unity.MLAgents.Agent>())
                          .Select(x => x.Replace("_", "").ToLower()).ToArray();

            // validate am
            if(!am.All(x => REQUIRED_ACTION_MEANINGS.Contains(x))) {
                string required_actions = string.Join(",", REQUIRED_ACTION_MEANINGS);
                string actual_actions = string.Join(",", am);
                throw new WorldOfBugsException(
                    $"Invalid actions [{actual_actions}] for Heuristic {this.GetType().Name}, requires {required_actions}");
            }
        }
    }
}
