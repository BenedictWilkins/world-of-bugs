using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeBehaviour : MonoBehaviour {

    [Header("Rotation")]
    public Vector3 AngularAxis = Vector3.up;
    public float AngularSpeed = 0;
    [Space(10)]
    
    [Header("Movement")]
    public Vector3 MovementExtent = Vector3.up * 2;
    public Vector3 MovementAngleOffset = Vector3.zero;
    public float MovementSpeed = 10;

    

    protected Vector3 InitialPosition;

    void OnEnable() {
        InitialPosition = transform.position;
    }

    void Update() {
        transform.RotateAround(transform.position, AngularAxis, AngularSpeed * Time.deltaTime);
        Vector3 motion = Sin((Vector3.one * MovementSpeed * Time.time) + MovementAngleOffset);
        motion.Scale(MovementExtent);
        transform.position = InitialPosition + motion;
    }

    protected Vector3 Sin(Vector3 vec) {
        return new Vector3(Mathf.Sin(vec[0]), Mathf.Sin(vec[1]), Mathf.Sin(vec[2]));
    }
}
