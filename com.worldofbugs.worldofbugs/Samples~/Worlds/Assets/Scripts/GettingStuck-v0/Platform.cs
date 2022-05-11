using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WorldOfBugs;

public class Platform : MonoBehaviour {
   
    public Vector3 MovementExtent = Vector3.up * 2;
    public Vector3 MovementAngleOffset = Vector3.zero;
    public float MovementSpeed = 10;
    public bool IgnoreGravity = false; // ignore gravity?

    protected Vector3 InitialPosition;
    protected BoxCollider triggerCollider;
    
    protected HashSet<Collider> OnPlatform = new HashSet<Collider>();
    protected float time = 0f;

    void Awake() {
        InitialPosition = transform.position;
        if (triggerCollider == null) {
            InitializeTriggerCollider();
        }
    }

    protected virtual void InitializeTriggerCollider() {
        triggerCollider = gameObject.AddComponent<BoxCollider>(GetComponent<BoxCollider>());
        triggerCollider.isTrigger = true;
        float yscale = 1.05f;
        Vector3 size = triggerCollider.size;
        Vector3 center = triggerCollider.center;
        size.Scale(new Vector3(1f,yscale,1f));
        center += (size - triggerCollider.size) / 2;
        triggerCollider.size = size;
        triggerCollider.center = center;
    }

    void Update() {
        time += Time.deltaTime;
        Vector3 motion = Sin((Vector3.one * MovementSpeed * time) + MovementAngleOffset);
        motion.Scale(MovementExtent);
        Vector3 newPosition = InitialPosition + motion;
        
        // used to update the position of the player
        Vector3 dPosition = newPosition - transform.position;
        // TODO support different gravity directions ? :)
        dPosition.y *= Convert.ToSingle((Mathf.Sign(dPosition.y) > 0) || IgnoreGravity);

        foreach (Collider collider in OnPlatform) {
            //Vector3.MoveTowards(collider.gameObject.transform.position, gameObject.transform.position);
            collider.gameObject.transform.position = collider.gameObject.transform.position + dPosition;
        }
        
        transform.position = newPosition;
    }

    void OnTriggerEnter(Collider collider) {
        OnPlatform.Add(collider);
    }

    void OnTriggerExit(Collider collider) {
        OnPlatform.Remove(collider);
    }

    protected Vector3 Sin(Vector3 vec) {
        return new Vector3(Mathf.Sin(vec[0]), Mathf.Sin(vec[1]), Mathf.Sin(vec[2]));
    }
}
