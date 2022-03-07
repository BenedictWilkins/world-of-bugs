﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

using GD.MinMaxSlider;

public class ScreenTear : Bug {

    [Tooltip("Camera RenderTexture to apply the screen tear to.")]
    public RenderTexture _cameraRenderTexture;
    [Tooltip("Bug Mask RenderTexture that will label the tear.")]
    public RenderTexture _bugMaskRenderTexture;

    [Range(1,8), Tooltip("Number of frames to tear into, the effect will be more pronounced with larger n.")]
    public int n = 1;

    [MinMaxSlider(0f,10f)]
    public Vector2 deltaRange = new Vector2(0.1f, 1f);

    [SerializeField, MinMaxSlider(0f, 1f), Tooltip("Y position of the top/bottom of the tear.")]
    private Vector2 TearMinMax = new Vector2(0.2f, 0.3f);
    [Tooltip("Whether to randomly modify the tear coordinates.")]
    public bool random = true;


    protected float TearMin { 
        get { return TearMinMax.x; }
        set { TearMinMax.x = value; 
              cameraPostEffect.TearMin = value;
              bugMaskCameraPostEffect.TearMin = value; }
    }
    protected float TearMax {
        get { return TearMinMax.y; }
        set { TearMinMax.y = value; 
              cameraPostEffect.TearMax = value;
              bugMaskCameraPostEffect.TearMax = value; }
    }

    protected ScreenTearPost cameraPostEffect;
    protected ScreenTearPost bugMaskCameraPostEffect;

    void Awake() {
        Camera[] cameras = CameraExtensions.GetCamerasByRenderTexture(_cameraRenderTexture);
        Camera _camera = cameras[0];

        cameras = CameraExtensions.GetCamerasByRenderTexture(_bugMaskRenderTexture);
        Camera _bugMaskCamera = cameras[0];


        // find cameras, add various components for on post render
        // TODO this camera history should be static somewhere!!! when introducing freezing, this will be important.
        CameraHistory history = _camera.gameObject.AddComponent<CameraHistory>();
        history.n = n;

        cameraPostEffect = _camera.gameObject.AddComponent<ScreenTearPostCamera>();
        cameraPostEffect.Initialise(this, history);

        bugMaskCameraPostEffect = _bugMaskCamera.gameObject.AddComponent<ScreenTearPostBugMaskCamera>();
        bugMaskCameraPostEffect.Initialise(this, history);   
        StartCoroutine("EnableDisable");
    }

    public override void OnDisable() {}
    public override void OnEnable() {}

    IEnumerator EnableDisable() {
        while (true) {
            if (random) {    
                TearMin = UnityEngine.Random.Range(0f, 0.9f);
                TearMax = Mathf.Min(TearMin + UnityEngine.Random.Range(0.1f, 0.7f), 1f);
            }
            cameraPostEffect._enable = ! cameraPostEffect._enable;
            bugMaskCameraPostEffect._enable = ! bugMaskCameraPostEffect._enable;
            yield return new WaitForSeconds(UnityEngine.Random.Range(deltaRange.x, deltaRange.y));
        }
    }

    public override bool InView(Camera camera) { 
        return false; // TODO
    }

    public class ScreenTearPost : MonoBehaviour {
        
        public bool _enable = true;

        public float TearMin {
            get { return _bug.TearMinMax.x; }
            set { _material.SetFloat("_TearMin", value); }
        }
        public float TearMax {
            get { return _bug.TearMinMax.y; }
            set { _material.SetFloat("_TearMax", value); }
        }
        public int n {
            get { return _bug.n; }
        }
        public Color BugType {
            get { return _bug.gameObject.GetComponent<BugTag>().bugType; }
        }

        protected ScreenTear _bug;
        protected Material _material;
        protected CameraHistory _history;

        public void Initialise(ScreenTear bug, CameraHistory history) { 
            if (_bug == null) { 
                _bug = bug;
                _history = history;
                TearMin = _bug.TearMinMax.x;
                TearMax = _bug.TearMinMax.y;
            }
        }
    }

    public class ScreenTearPostCamera : ScreenTearPost {

        void Awake() { 
            _material = new Material(Shader.Find("Bug/ScreenTear"));
        }

        void OnRenderImage(RenderTexture src, RenderTexture dst) {
            if (_enable && Time.frameCount > 10 + n) {
                _material.SetTexture("_TearTex", _history[n-1]);
                //_material.SetTexture("_MainTex", src);
                Graphics.Blit(src, dst, _material);
            } else {
                Graphics.Blit(src, dst);
            }
        }
    }

    public class ScreenTearPostBugMaskCamera : ScreenTearPost {

        void Awake() { 
            _material = new Material(Shader.Find("Bug/ScreenTearBugMask"));
        }

        void Start() {
            _material.SetColor("_BugType", (Color32) BugType);
        }

        void OnRenderImage(RenderTexture src, RenderTexture dst) {
            if (_enable && Time.frameCount > 10 + n) {
                _material.SetTexture("_CameraTex", _history[0]); // current rendering of the main camera
                _material.SetTexture("_TearTex", _history[n-1]); // previous rendering of main camera
  
                Graphics.Blit(src, dst, _material);
            } else {
                Graphics.Blit(src, dst);
            }
        }
    }



}