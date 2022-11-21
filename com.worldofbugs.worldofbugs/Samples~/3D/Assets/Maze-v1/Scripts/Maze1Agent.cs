using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

using WorldOfBugs;


public class Maze1Agent : WorldOfBugs.AgentDefault {

    public NavMeshSurface[] surfaces;

    public override void Reset() {
        // choose a random position within the bounds of the maze
    }

    [Action]
    public void none() {} // does nothing

    [Action]
    public void forward() {
        Vector3 movement = gameObject.transform.forward * Time.deltaTime * MovementSpeed;
        gameObject.transform.Translate(movement,
                                       Space.World); // TODO change this to move position?
    }

    [Action]
    public void rotate_left() {
        gameObject.transform.Rotate(new Vector3(0, -1 * Time.deltaTime * AngularSpeed, 0));
    }

    [Action]
    public void rotate_right() {
        gameObject.transform.Rotate(new Vector3(0, 1 * Time.deltaTime * AngularSpeed, 0));
    }
}
