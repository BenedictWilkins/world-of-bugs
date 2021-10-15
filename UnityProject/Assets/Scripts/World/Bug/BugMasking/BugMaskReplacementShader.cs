using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BugMaskReplacementShader : MonoBehaviour {
    public Shader ReplacementShader;
    public bool debug;

    void Start() {
        GameObject parent = gameObject.transform.parent.gameObject;
        bool isFirstChild = parent.GetComponent<BugMaskReplacementShader>() == null;
        if (Application.isEditor && debug && isFirstChild) {
            // create a clone and make it render to the second display...
            GameObject debugCamera = Instantiate(gameObject);
            debugCamera.transform.parent = gameObject.transform;
            Camera _debugCamera = debugCamera.GetComponent<Camera>();
            _debugCamera.targetTexture = null;
            _debugCamera.targetDisplay = 1;
        }
    }

    void Awake() {
        if (ReplacementShader != null) {
           GetComponent<Camera>().SetReplacementShader(ReplacementShader, "RenderType");
        }     
    }

    void OnDestroy() {
        GetComponent<Camera>().ResetReplacementShader();
    }
}
