using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

using WorldOfBugs;

public class BoundaryHole : Bug {

    public AgentFirstPerson _player;
    public GameObject level;
    private GameObject _go; // disable colldier on this game object

    void Awake() {
        _player.CameraBugMask.backgroundColor = bugType;
    }

    public override void OnEnable() {
        Transform[] children = level.transform.GetComponentsInChildren<Transform>(true);
        children = Array.FindAll(children, x => x.GetComponent<Collider>() != null); // leaf children
        int j = UnityEngine.Random.Range(0, children.Length); 
        _go = children[j].gameObject;
        _go.GetComponent<Collider>().enabled = false;
    }

    public override void  OnDisable() {
        if (_go != null) {
            _go.GetComponent<Collider>().enabled = true;
        }
    }

    void OnDrawGizmos() {
        if (_go != null) {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(_go.transform.position, .3f);
        }
    }

}
