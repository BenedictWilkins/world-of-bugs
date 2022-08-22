using UnityEngine;
using NaughtyAttributes;

namespace WorldOfBugs {

    [ExecuteInEditMode]
    public class ReplacementShader : MonoBehaviour {

        [Tooltip("Shader to use.")]
        public Shader Shader;
        [ReadOnly, Tooltip("Camera to render masked view of the environment.")]
        private Camera Camera;

        [Button]
        public void Default() {
            Camera?.ResetReplacementShader();
        }

        [Button]
        public void Replace() {
            Camera = GetComponent<Camera>();
            Camera.SetReplacementShader(Shader, "RenderType");
        }

        void Awake() {
            Replace();
        }

        void OnDestroy() {
            Default();
        }
    }
}
