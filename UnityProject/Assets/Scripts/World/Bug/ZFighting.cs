using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ZFighting : Bug { 

    public GameObject original;
    protected GameObject clone;

    public void Awake() {
        clone = Instantiate(original);
        clone.transform.parent = gameObject.transform;

        BugTag tag = GetComponent<BugTag>();
        Transform[] children = gameObject.transform.GetComponentsInChildren<Transform>(true);
        children = Array.FindAll(children, x => x.GetComponent<Renderer>() != null); // leaf children

        foreach (Transform child in children) {
            // invert all the textures, this will cause some z-fighting if the game object is placed exactly on a duplicate.
            Vector3 ls = child.localScale;
            child.localScale = new Vector3(-ls.x, ls.y, ls.z);
            // tag children for rendering with the BugMask.
            tag.Tag(child.gameObject);
            // we only want to render the game object, all other components should be disabled
            foreach (Behaviour c in child.GetComponents<Behaviour>()) {
                c.enabled = false;
            }
            foreach (Collider c in child.GetComponents<Collider>()) {
                c.enabled = false;
            }
            child.GetComponent<Renderer>().enabled = true;
        }
    }

    public override void OnEnable() {
        // ensure that all children are inactive
        Transform[] children = gameObject.transform.GetComponentsInChildren<Transform>(true);
        foreach (Transform c in children) {
            if (c != gameObject.transform) { // dont turn yourself off again...
                c.gameObject.SetActive(false);
            }
        }
        children = Array.FindAll(children, x => x.GetComponent<Renderer>() != null).ToArray();
        // randomly choose a child to activate
        int i = UnityEngine.Random.Range(0, children.Length); 
        Transform child = children[i];
        while (child != this.transform) {
            child.gameObject.SetActive(true);
            child = child.parent;
        }
    }

    public override void OnDisable() {
        // TODO disable all children? 
        Transform[] children = gameObject.transform.GetComponentsInChildren<Transform>(true);
        foreach (Transform c in children) {
            if (c != gameObject.transform) { // dont turn yourself off again...
                c.gameObject.SetActive(false);
            }
        }
    }

    public override bool InView(Camera camera) { 
        if (gameObject.activeSelf) {
             // shouldnt call this too often its a bit expensive? 
            BugTag tag = GetComponent<BugTag>(); 
            int[] mask = BugMask.Instance.Mask(camera); 
            // Compare the mask with my bug type...
            bool result = mask.Contains((int) tag.bugType);
            return result;
        }
        return false;
    }
}
