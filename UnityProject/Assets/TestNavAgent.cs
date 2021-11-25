using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TestNavAgent : MonoBehaviour {

    public NavMeshSurface[] surfaces;

    private NavMeshPath path;
    private Vector3 nextPosition { get { return path.corners[1]; }}
    private Vector3 goalPosition { get { return path.corners[path.corners.Length-1]; }}

    private Vector3 targetPosition;
    private Vector3 currentPosition { get { return transform.position; }}

    

    void Start() {
        BuildNavMesh();
        path = new NavMeshPath();
        targetPosition = nextPoint();
        NavMesh.CalculatePath(currentPosition, targetPosition, NavMesh.AllAreas, path);
    }

    void Update() {
        transform.position = Vector3.MoveTowards(transform.position, nextPosition, Time.deltaTime); 
        BuildNavMesh();
        UpdateNavPath();
    }

    void BuildNavMesh() {
        foreach (NavMeshSurface surface in surfaces) {
            surface.BuildNavMesh();
        }
    }

    void UpdateNavPath() {
        Debug.Log(Vector3.Distance(currentPosition, goalPosition));
        
        if (Vector3.Distance(currentPosition, goalPosition) < 0.1) {
            targetPosition = nextPoint();
        }
        NavMesh.CalculatePath(currentPosition, targetPosition, NavMesh.AllAreas, path);
        for (int i = 0; i < path.corners.Length - 1; i++) {
            Debug.DrawLine(path.corners[i], path.corners[i + 1], Color.red);
        }
    }

    void OnDrawGizmos() {
        if (path != null) {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(nextPosition, .3f);
        }
    }

    public Vector3 nextPoint() {
        Vector3 randomDirection = Random.insideUnitSphere * 2 + currentPosition;
        NavMeshHit hit;
        Vector3 finalPosition = Vector3.zero;
        if (NavMesh.SamplePosition(randomDirection, out hit, 10, 1)) {
            finalPosition = hit.position;            
        }
        return finalPosition;
     }
}
