using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.IO;

public class ScreenCapture : MonoBehaviour {

    public int[] displays = { 0 };
    public int count;
    public KeyCode screenshotKey = KeyCode.F9;
    public string fileName = "screen";

    void LateUpdate() {
        if (Input.GetKeyDown(screenshotKey)) {
            foreach (int display in displays) {
                ScreenShot(display);
            }
        }
    }  

    public void ScreenShot(int display) {
        Texture2D image = ScreenCapture.Capture(Screen.width, Screen.height, display);
        byte[] bytes = image.EncodeToPNG();
        Destroy(image);
        string path = $"{Application.dataPath}/Captures/display{display+1}/{fileName}({count}).png";
        File.WriteAllBytes(path, bytes);
        count++;
        Debug.Log($"Saved Screen Capture: {path}");
    }

    public static Texture2D Capture(int width, int height, int display) {
        List<Camera> cameras = new List<Camera>(Camera.allCameras);
        cameras = cameras.FindAll(x => x.targetDisplay == display).ToList();
        cameras.Sort((c1,c2) => (int)c1.depth - (int)c2.depth);
        RenderTexture renderTexture = new RenderTexture(width, height, 24);
        RenderTexture.active = renderTexture;  
        foreach (Camera camera in cameras) {
            // dont capture cameras that are already rendering to a target texture...
            if (camera.enabled && camera.targetTexture == null) {
                //float fov = camera.fov;
                camera.targetTexture = renderTexture;
                camera.Render();
                camera.targetTexture = null;
                //camera.fov = fov;
            }
        }
 
        Texture2D result = new Texture2D(width, height, TextureFormat.ARGB32, false);
        result.ReadPixels(new Rect(0.0f, 0.0f, width, height), 0, 0, false);
        result.Apply();
        RenderTexture.active = null;
        Destroy(renderTexture);
        return result;
    }

 
}