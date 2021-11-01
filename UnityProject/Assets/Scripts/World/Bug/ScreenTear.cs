using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenTear : MonoBehaviour {

    public Camera _camera;

    void Awake() {
        _camera.gameObject.AddComponent<ScreenTearPost>();
    }
}
