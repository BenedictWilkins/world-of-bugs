using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BugMask : MonoBehaviour {

    public static BugMask _instance;
    public static BugMask Instance { get { return _instance; } }

    private void Awake() {
        if (_instance != null && _instance != this) {
            Destroy(this.gameObject);
        } else {
            _instance = this;
        }
    }

    protected struct MaskData {
        public int[] mask;
        public int frame;
    }
    
    protected Dictionary<int, MaskData> _masks = new Dictionary<int, MaskData>(); // map from RenderTexture GUID to pixel array (mask)
    
    public int[] Mask(Camera camera) {
        RenderTexture _renderTexture = camera.targetTexture;
        if (camera.targetTexture == null) {
            throw new NullReferenceException("Camera does not contain a RenderTexture.");
        }
        int _id = _renderTexture.GetInstanceID();
        int _frame = -1;
        if (_masks.ContainsKey(_id)) {
            _frame = _masks[_id].frame;
        }

        if (_frame != Time.frameCount) {
            _frame = Time.frameCount;
            int[] _mask = GenerateMask(_renderTexture);
            _masks[_id] = new MaskData() {mask = _mask, frame = _frame};
            return _mask;
        } else {
            return _masks[_id].mask;
        }
    }

    public int[] GenerateMask(RenderTexture renderTexture) {
        Texture2D tex = new Texture2D(renderTexture.width, renderTexture.height);
        RenderTexture.active = renderTexture;
        tex.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0, true);
        tex.Apply();
        Color32[] rgbs = tex.GetPixels32(0);
        // convert pixels to integers, ignore alpha channels
        int[] pixs = new int[rgbs.Length];
        for (int i = 0; i < pixs.Length; i++) {
            pixs[i] = 0xFFFF * rgbs[i].r + 0xFF * rgbs[i].g + rgbs[i].b; // TODO this must match the BugType conversion operator implementation...
        }
        return pixs;

    }
}
