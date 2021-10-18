using UnityEditor;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

class BugMaskCameraPreview : EditorWindow {

    protected Camera camera;

    [MenuItem ("Window/BugMaskPreview")]
    public static void ShowWindow() {
        EditorWindow editorWindow = GetWindow(typeof(BugMaskCameraPreview));
        editorWindow.autoRepaintOnSceneChange = true;
        editorWindow.Show();
        editorWindow.minSize = new Vector2(10,10);
    }

    void OnEnable() {
        // find camera that contains the bug mask render texture...
        Camera[] cameras = CameraExtensions.GetBugMaskCameras();
        camera = cameras[0]; // TODO multiple camera support?
    }

    void OnGUI() {
        RenderTexture mask = camera.targetTexture;
        GUI.DrawTexture(GetBounds(), mask);    
    }

    Rect GetBounds() {
        float w = camera.targetTexture.width * 2;
        float h = camera.targetTexture.height * 2;

        float dw = position.width - w;
        float dh = position.height - h;

        return new Rect(dw / 2, dh / 2, w, h);
    }
}