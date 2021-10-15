using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ZFighting : Bug {

    public static readonly string LAYERTAG = "ZFighting";
    public static readonly string BUGTAG = "ZBug";

    public GameObject level;
    public GameObject camera;

    protected GameObject zCamera;
    protected int layer = -1;

    protected GameObject _fighter;
    protected int _oldlayer = -1;

    void Awake() {
        layer = LayerMask.NameToLayer(LAYERTAG);
        if (layer < 0) {
            // if this happens something has gone very wrong.. probably the zfighting camera prefab is also broken...
            throw new KeyNotFoundException($"\"{ZFighting.LAYERTAG}\" must be a layer, please add it to enable the ZFighting bug.");
        }
        foreach (Camera c in camera.GetComponentsInChildren<Camera>()) {
            HideLayer(c, layer); // hide the zfighting layer if its not already hidden...
        }
        zCamera = new GameObject("ZFightingCamera");
        Camera _zCamera = zCamera.AddComponent<Camera>();
        _zCamera.CopyFrom(camera.GetComponent<Camera>());
        zCamera.transform.parent = camera.transform;
        foreach (Camera c in zCamera.GetComponentsInChildren<Camera>()) {
            _MakeZCamera(c);
        }
        GetComponent<BugTag>().bugTag = BUGTAG;
    }
    
    protected void _MakeZCamera(Camera camera) {
        // set up zcamera properties
        camera.cullingMask = 0; // hide all layers
        ShowLayer(camera, layer); // show only the zfighting layer
        camera.clearFlags = CameraClearFlags.Depth; 
        camera.depth = camera.depth + 1;
    }

    public override void Enable() {
        BugTag tag = GetComponent<BugTag>();
        // get children of the given game object (level)
        Transform[] children = level.transform.GetComponentsInChildren<Transform>(true);
        children = Array.FindAll(children, x => x.GetComponent<Renderer>() != null); // leaf children
        int i = UnityEngine.Random.Range(0, children.Length); 
        _fighter = children[0].gameObject; //TODO 0 -> i
        _oldlayer = _fighter.layer;
        _fighter.layer = layer;
        tag.Tag(_fighter);
    }

    public override void Disable() {
        if (_fighter != null) {
            Material material = _fighter.GetComponent<Renderer>().material;
            BugTag tag = GetComponent<BugTag>();
            tag.Untag(_fighter);
            _fighter.layer = _oldlayer;
            _fighter = null;
        }
    }

    public override bool InView(Camera camera) { 
        if (gameObject.activeSelf) {
            BugTag tag = GetComponent<BugTag>(); 
            int[] mask = BugMask.Instance.Mask(camera); 
            // Compare the mask with my bug type...
            bool result = mask.Contains((int) tag.bugType);
            return result;
        }
        return false;
    }

    private void ShowLayer(Camera camera, int layer) {
        camera.cullingMask |= 1 << layer;
    }

    private void HideLayer(Camera camera, int layer) {
        camera.cullingMask &=  ~(1 << layer);
    }
}


