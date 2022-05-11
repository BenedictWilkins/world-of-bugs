using UnityEngine;

namespace WorldOfBugs {
    
    /* <summary>
        Attach a replacement shader to the bug masking camera.
        
        NOTE:because setting shader properties requires a global call (Shader.SetGlobal...), things might go wrong up if multiple agents are in use ...s
    </summary> */
    [ExecuteInEditMode]
    public class BugMaskReplacementShader : MonoBehaviour {

        [Tooltip("Bug mask shader to use.")]
        public Shader MaskShader;
        [Tooltip("Camera to render masked view of the environment.")]
        public Camera Camera;
        public RenderTexture MaskTexture { get { return Camera.targetTexture; }}

        void Awake() {
            Camera = GetComponent<Camera>();
            Camera.SetReplacementShader(MaskShader, "RenderType");
        }

        void OnDestroy() {
            if (Camera != null) {
                Camera.ResetReplacementShader();
            }
        }
    }
}