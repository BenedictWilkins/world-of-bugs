using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvisibleWall : MonoBehaviour {

    public GameObject level;
    private Bounds bounds;

    void Start() {
        Bounds bounds = CreateBoundingBox(level);
    }

    void Update() {
    }

    public static Vector3 RandomPointInBounds(Bounds bounds) {
        return new Vector3(
                   Random.Range(bounds.min.x, bounds.max.x),
                   Random.Range(bounds.min.y, bounds.max.y),
                   Random.Range(bounds.min.z, bounds.max.z)
               );
    }


    Bounds CreateBoundingBox(GameObject level) {
        Bounds bounds = new Bounds(level.transform.position, Vector3.zero);
        Transform[] allDescendants = level.GetComponentsInChildren<Transform>();

        foreach(Transform desc in allDescendants) {
            Renderer childRenderer = desc.GetComponent<Renderer>();

            if(childRenderer != null) {
                bounds.Encapsulate(childRenderer.bounds);
            }
        }

        return bounds;
    }


}
