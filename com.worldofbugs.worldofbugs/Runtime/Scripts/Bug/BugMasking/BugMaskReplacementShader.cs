using UnityEngine;


// attach to a the masking camera to use the the given shader
[ExecuteInEditMode]
public class BugMaskReplacementShader : MonoBehaviour {

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
