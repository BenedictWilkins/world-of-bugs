using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingCube : MonoBehaviour {

    public float magnitude = 1;

    void FixedUpdate() {
        transform.position = new Vector3(transform.position.x, magnitude * Mathf.Sin(Time.time), transform.position.z);
    }
    
}
