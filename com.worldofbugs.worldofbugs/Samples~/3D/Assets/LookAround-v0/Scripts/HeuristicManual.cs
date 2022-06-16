using System.Linq;
using UnityEngine;

using Unity.MLAgents.Actuators;
using WorldOfBugs;

namespace WorldOfBugs.LookingAround {

    public class HeuristicManual : HeuristicComponent {

        public override void Heuristic(in ActionBuffers buffer) {
            var _buffer = buffer.ContinuousActions;
            _buffer[0] = Input.GetAxisRaw("Vertical");
            _buffer[1] = Input.GetAxisRaw("Horizontal");
        }
    }
}
