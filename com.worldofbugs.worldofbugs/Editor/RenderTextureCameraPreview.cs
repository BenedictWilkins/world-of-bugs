using UnityEditor;
using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Linq;



class RenderTextureCameraPreview : EditorWindow {

    private Camera[] cameras;

    [MenuItem ("Window/Preview/RenderTextureCameraPreview")]
    public static void ShowWindow() {
        EditorWindow editorWindow = GetWindow(typeof(RenderTextureCameraPreview));
        editorWindow.autoRepaintOnSceneChange = true;
        editorWindow.Show();
        editorWindow.minSize = new Vector2(10,10);
    }

    void OnEnable() {
        cameras = Array.FindAll(Camera.allCameras, x => x.targetTexture != null).ToArray();
    }

    void OnGUI() {
        if (cameras?.Length == 0) {
            OnEnable(); // something happened and the cameras were destroyed, find them again!
        }
        // TODO this doesnt render properly if cameras.Length > 2...
        for (int i = 0; i < cameras.Length; i++) {
            if (cameras[i] != null) {
                RenderTexture tex = cameras[i].targetTexture;
                Rect bounds = GetBounds(i, tex.width / tex.height);
                //Debug.Log($"{i} {bounds}");
                GUI.DrawTexture(bounds, tex);   
            } else {
                OnEnable();
            }
        }
    }

    Rect GetBounds(int index, float aspect) {
        int n = (int) Mathf.Ceil(Mathf.Sqrt(cameras.Length));
        int m = (cameras.Length % n) + 1;

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
