using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformSpin : MonoBehaviour {

    public float DAngle = 1;
    public HashSet<GameObject> On = new HashSet<GameObject>();
    public Vector3 axis = new Vector3(0, 1, 0);
    protected float nextRotation;
    public bool indicator = false;

    public float NextRotation {
        get {
            return nextRotation;
        }
        set {
            nextRotation = value;
            update_indicator();
        }
    }


    void Awake() {
        NextRotation = Random.Range(-1, 2);
        update_indicator();
    }

    // Update is called once per frame
    void FixedUpdate() {
        float angle = NextRotation * DAngle;
        transform.Rotate(axis * angle, Space.World);

        foreach(GameObject obj in On) {
            obj.transform.RotateAround(transform.position, axis, angle);
        }

        Debug.Log(On.Count);
        // create an indicator of the next turn direction
        NextRotation = Random.Range(-1, 2);
    }

    void update_indicator() {
        if(indicator) {
            byte v = (byte)(((NextRotation + 1) / 2) * 255);
            Color c = new Color32(v, v, v, 1);
            GetComponent<Renderer>().material.SetColor("_Color", c);
        }
    }

    void OnTriggerEnter(Collider collider) {
        On.Add(collider.gameObject);
    }

    void OnTriggerExit(Collider collider) {
        On.Remove(collider.gameObject);
    }
}
