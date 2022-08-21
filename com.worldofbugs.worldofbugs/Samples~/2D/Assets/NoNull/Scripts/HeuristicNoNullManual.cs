using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WorldOfBugs;
using Unity.MLAgents.Actuators;

public class HeuristicNoNullManual : HeuristicComponent {

    private bool deciding = false;

    public override void Heuristic(in ActionBuffers buffer) {
        Debug.Log($"Deciding: {deciding}");

        if(deciding) {
            throw new WorldOfBugsException("Manual heuristic was still deciding on an action but Heuristic got called again.");
        }

        var _buffer = buffer.DiscreteActions;
        _buffer[0] = -1; // indicates that no decision has been made...
        deciding = true;
        StartCoroutine(WaitForInput(
                           buffer)); // this should not be called again unless the coroutine has finished!
    }

    public IEnumerator WaitForInput(ActionBuffers buffer) {
        int leftright = (int) Mathf.Round(Input.GetAxis("Horizontal"));
        int forwardback = (int) Mathf.Round(Input.GetAxis("Vertical"));
        var _buffer = buffer.DiscreteActions;

        while(leftright + forwardback == 0) {
            leftright = (int) Mathf.Round(Input.GetAxis("Horizontal"));
            forwardback = (int) Mathf.Round(Input.GetAxis("Vertical"));
            yield return null;
        }

        if(forwardback > 0) {
            _buffer[0] = 0; // up
        } else if(forwardback < 0) {
            _buffer[0] = 1; // down
        } else if(leftright < 0) {
            _buffer[0] = 2; // left
        } else if(leftright > 0) {
            _buffer[0] = 3; // right
        }

        deciding = false;
    }

}
