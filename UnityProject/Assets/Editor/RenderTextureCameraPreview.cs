using UnityEditor;
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

class RenderTextureCameraPreview : EditorWindow {

    private Camera[] cameras;
    private int n;
    private int m;

    [MenuItem ("Window/RenderTextureCameraPreview")]
    public static void ShowWindow() {
        EditorWindow editorWindow = GetWindow(typeof(RenderTextureCameraPreview));
        editorWindow.autoRepaintOnSceneChange = true;
        editorWindow.Show();
        editorWindow.minSize = new Vector2(10,10);
    }

    void OnEnable() {
        cameras = Array.FindAll(Camera.allCameras, x => x.targetTexture != null).ToArray();
        n = (int) Mathf.Ceil(Mathf.Sqrt(cameras.Length));
        m = (cameras.Length % n) + 1;
    }

    void OnGUI() {

        if (cameras.Length == 0 || cameras[0] == null) {
            OnEnable(); // something happened and the cameras were destroyed, find them again!
        }
        for (int i = 0; i < cameras.Length; i++) {
            RenderTexture tex = cameras[i].targetTexture;
            //Debug.Log($"{tex.width} {tex.height} {tex}");
            GUI.DrawTexture(GetBounds(i, tex.width / tex.height), tex);   
        }
    }

    Rect GetBounds(int index, float aspect) {
        int nn = n;
        int mm = m;
        // flip vertical/horizontal layout
        if (position.width < position.height) {
            nn = m;
            mm = n;
        }
        // find initial bounding box in grid
        int w = (int) (position.width / nn);
        int h = (int) (position.height / mm);
        int xi = index % nn;
        int yi = index / nn;
        int x = w * xi;
        int y = h * yi;
        // use aspect to fit
        int wh = Mathf.Min(w, h);
        int wa = (int)( wh * aspect);
        int ha = (int)( wh * (1/aspect));
        x = x + (w - wa) / 2;
        y = y + (h - ha) / 2;
        return new Rect(x, y, wa, ha);
    }
}