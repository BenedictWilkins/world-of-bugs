using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents.Actuators;
using WorldOfBugs;

public class NoNullAgent : Agent {

    public float CellSize = 1f;
    public float CellOffset = 0.5f;

    protected Vector3 initialPosition;

    public void Awake() {
        initialPosition = transform.position;
    }

    public override void Reset() {
        transform.position = initialPosition;
    }

    public void Update() {
        // position to grid
        transform.position = Round(transform.position / CellSize) * CellSize +
                             (Vector3.one * CellOffset);
    }

    public new void FixedUpdate() {
        // NOTE: the order of this is important.
        // if RequestDecision comes after EndEpisodes, any reward obtained in this "step"
        // will not be sent to python and the "intial state" (after reset) will be sent as the final state...
        // check the action buffer
        ActionBuffers buffer = GetStoredActionBuffers();

        // print(buffer.DiscreteActions[0]);
        if(buffer.DiscreteActions[0] >= 0) {
            Debug.Log("Test");
            RequestDecision();
        }

        foreach(IReset reset in Resets) {
            if(reset.ShouldReset(gameObject)) {
                EndEpisode();
                break;
            }
        }
    }

    [Action]
    public void up() {
        transform.position = new Vector3(transform.position.x, transform.position.y + CellSize,
                                         transform.position.z);
    }

    [Action]
    public void down() {
        transform.position = new Vector3(transform.position.x, transform.position.y - CellSize,
                                         transform.position.z);
    }

    [Action]
    public void left() {
    }

    [Action]
    public void right() {
        //
    }

    public static Vector3 Round(Vector3 vec) {
        return new Vector3(Mathf.Round(vec.x), Mathf.Round(vec.y), Mathf.Round(vec.z));
    }



}
