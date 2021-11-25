using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class BoundaryHole : Bug {

    public Player _player;
    public GameObject level;
    private GameObject _go; // disable colldier on this game object

    void Awake() {
        _player.CameraBugMask.backgroundColor = GetComponent<BugTag>().bugType;
    }

    void OnEnable() {
        Transform[] children = level.transform.GetComponentsInChildren<Transform>(true);
        children = Array.FindAll(children, x => x.GetComponent<Collider>() != null); // leaf children
        int j = UnityEngine.Random.Range(0, children.Length); 
        _go = children[j].gameObject;
        _go.GetComponent<Collider>().enabled = false;
    }

    void OnDisable() {
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

    public override bool InView(Camera camera) {
        return false;
    }

    void Start() {
        // compute a collection of bounding boxes around the level
        //Collider boxCol = CreateBoundingBox(level);
        //if (!boxCol.isTrigger) {
        //    throw new Exception($"The Collider attached to the {level} must be a trigger for the {typeof(BoundaryHole).Name} bug to work properly.");
        //}
        // add a skyboxbug to the game object, on trigger exit
        //SkyBoxBugTrigger trigger = level.AddComponent<SkyBoxBugTrigger>();
        //trigger.Initialise(GetComponent<BugTag>(), _player.CameraBugMask);
    }


    // ************************* // 
    // NOT USED? 
    // ************************* // 

    Collider CreateBoundingBox(GameObject level) {
        BoxCollider boxCol = level.GetComponent<BoxCollider>();
        if (boxCol == null) {
            boxCol = level.AddComponent<BoxCollider>();
            Bounds bounds = new Bounds(level.transform.position, Vector3.zero);
            Transform[] allDescendants = level.GetComponentsInChildren<Transform>();
            foreach (Transform desc in allDescendants) {
                Renderer childRenderer = desc.GetComponent<Renderer>();
                if (childRenderer != null) {
                    bounds.Encapsulate(childRenderer.bounds);
                }
                boxCol.center = bounds.center - level.transform.position;
                boxCol.size = bounds.size;
            }
            boxCol.isTrigger = true;
        }
        return boxCol;
    }

    public class SkyBoxBugTrigger : MonoBehaviour {

        private BugTag _tag;
        private Camera _camera;

        public void Initialise(BugTag tag, Camera camera) {
            _tag = tag;
            _camera = camera;
        }

        void OnTriggerEnter(Collider other) {
            Debug.Log("TriggerEnter");
        }
        
        void OnTriggerExit(Collider other) {
            Debug.Log("TriggerExit");
        }
    }


    // ************************* // 

   
}
