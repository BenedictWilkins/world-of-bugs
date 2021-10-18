using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[ExecuteInEditMode]
public class BugMaskReplacementShader : MonoBehaviour {


    public Material[] postProcess;
    public Shader maskShader;

    protected Camera _camera;
    public RenderTexture MaskTexture { get { return _camera.targetTexture; }}


    void Awake() {
        _camera = GetComponent<Camera>();
        _camera.SetReplacementShader(maskShader, "RenderType");
    }

    void OnDestroy() {
        _camera.ResetReplacementShader();
    }
}
