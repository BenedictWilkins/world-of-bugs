using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpDown : MonoBehaviour
{

    public float magnitude = 1f;
    public float offset;

    // Update is called once per frame
    void Update()
    {

        float y = magnitude / 2 + transform.localScale.y / 2 + Mathf.Sin(offset + Time.time) * magnitude;
        transform.position = new Vector3(transform.position.x, y, transform.position.z);
    }
}
