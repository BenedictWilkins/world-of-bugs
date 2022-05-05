using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigidBodyLock : MonoBehaviour {

    protected RigidbodyConstraints old_constraints;

    public void OnEnable() {
        Rigidbody body = GetComponent<Rigidbody>();
        old_constraints = body.constraints;
        body.constraints = RigidbodyConstraints.FreezeAll;
    }

    public void OnDisable() {
        Rigidbody body = GetComponent<Rigidbody>();
        body.constraints = old_constraints;
    }
}
