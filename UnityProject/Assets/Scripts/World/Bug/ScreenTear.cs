using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenTear : Bug {

    [Tooltip("Camera to apply the screen tear to.")]
    public Camera _camera;
    [Range(1,8), Tooltip("Number of frames to tear into, the effect will be more pronounced with larger n.")]
    public int n = 1;

    [Range(0f,1f), Tooltip("Y position of the bottom of the tear.")]
    public float _tear1 = 0.2f;
    [Range(0f,1f), Tooltip("Y position of the top of the tear.")]
    public float _tear2 = 0.3f;

    protected ScreenTearPost postEffect;
 
    void Awake() {
        postEffect = _camera.gameObject.AddComponent<ScreenTearPost>();
        postEffect.TearY = new Vector2(_tear1, _tear2);
        postEffect.n = n;

        CameraHistory history = _camera.gameObject.GetComponent<CameraHistory>();
        history.n = n;
    }

    public override bool InView(Camera camera) { 
        return false; // TODO
    }
}
