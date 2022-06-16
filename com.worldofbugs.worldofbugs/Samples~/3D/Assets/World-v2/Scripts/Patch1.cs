using System;
using System.Reflection;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    This patches this issue with the moving cubes that pushes the player through the floor if underneath. With this patch the player is no longer able to go underneath the cubes.
*/

public class Patch1 : MonoBehaviour {

    public GameObject[] cubes;


    void OnEnable() {
        // To fix the out of bounds bug, add a collider under each cube.
        foreach(GameObject cube in cubes) {
            BoxCollider collider = cube.GetComponent<BoxCollider>();
            collider.size = new Vector3(collider.size[0], 10 * collider.size[1], collider.size[2]);
        }
    }


}
