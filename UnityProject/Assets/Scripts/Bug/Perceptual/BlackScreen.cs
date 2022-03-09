using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GD.MinMaxSlider;

public class BlackScreen : Bug {
    
    [Tooltip("Camera RenderTexture to apply the blackscreen to.")]
    public RenderTexture _cameraRenderTexture;
    [Tooltip("Bug Mask RenderTexture that will label black screen.")]
    public RenderTexture _bugMaskRenderTexture;
    
    [MinMaxSlider(0.01f, 0.1f), Tooltip("Number of seconds to remain black.")]
    public Vector2 frameRange = new Vector2(0.01f, 0.05f);

    [MinMaxSlider(0f,10f)]
    public Vector2 deltaRange = new Vector2(0.1f, 1f);

    protected bool _enabled = false;

    protected FillScreen[] fillScreens;

    void Awake() { 
        fillScreens = new FillScreen[2];
        Camera[] cameras = null;
        cameras = CameraExtensions.GetCamerasByRenderTexture(_cameraRenderTexture);
        fillScreens[0] = cameras[0].gameObject.AddComponent<FillScreen>();
        fillScreens[0].color = new Color(0f,0f,0f,1f);
        
        cameras = CameraExtensions.GetCamerasByRenderTexture(_bugMaskRenderTexture);
        fillScreens[1] = cameras[0].gameObject.AddComponent<FillScreen>();
        fillScreens[1].color = (Color) gameObject.GetComponent<BugTag>().bugType;
        StartCoroutine("EnableDisable");
    }

    public override void OnDisable() {}
    public override void OnEnable() {}

    IEnumerator EnableDisable() {
        while (true) {
            _enabled = ! _enabled;
            if (_enabled) { 
                StartCoroutine("toggler");
            }
            yield return new WaitForSeconds(UnityEngine.Random.Range(deltaRange.x, deltaRange.y));
        }
    }

    IEnumerator toggler() {
        while(_enabled) {
            foreach (FillScreen fs in fillScreens) {
                fs._enabled = ! fs._enabled;
            }
            yield return new WaitForSeconds(UnityEngine.Random.Range(frameRange.x, frameRange.y));
        }
        foreach (FillScreen fs in fillScreens) {
                fs._enabled = false;
        } 
    }

    public override bool InView(Camera camera) { 
        return false;
    }

   

    public class FillScreen : MonoBehaviour {
        [HideInInspector]
        public bool _enabled = false;
        [HideInInspector]
        public Color color {
            get { return _color;}
            set { 
                _color = value;
                _solid = new Texture2D(1,1); // dont need a massive texture, let uv handle that!
                _solid.SetPixels(new Color[] { value });
                _solid.Apply();
            }
        }

        protected Color _color;
        protected Texture2D _solid;

        [HideInInspector]
        public RenderTexture renderTexture;
        
        public void OnRenderImage(RenderTexture src, RenderTexture dst) {
            if (_enabled) {
                Graphics.Blit(_solid, dst);
            }  else {
                Graphics.Blit(src, dst);
            } 
            
        }
    }
}
