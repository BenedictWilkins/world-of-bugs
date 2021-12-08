using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DatasetRecorder : MonoBehaviour {

    public Camera[] cameras;

    protected int count = 0;
    protected int episode = 0;
    // screenshot stuff TODO remove...
    public KeyCode screenshotKey = KeyCode.F9;
    private int fileCount = 0;

    public string path { get { 
        string path = $"{Application.dataPath}/Captures/Preview/";
        if (!Directory.Exists(path)) {
            Directory.CreateDirectory(path);
        }
        return path;
    }}
    


    void Capture() {
        string _count = count.ToString("000000");
        string _episode = episode.ToString("0000");
        foreach (Camera camera in cameras) {
            string filename = $"{camera.gameObject.name}-{_count}.png";
            //string path = $"{path}/{_episode}/{filename}";
            //Debug.Log(path);
        }
        // TODO log agents actions aswell!
    }

    void CaptureCamera(Camera camera, string file) {
        RenderTexture renderTexture = camera.targetTexture;
        Texture2D image = new Texture2D(renderTexture.width, renderTexture.height);
        image.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
        image.Apply();
        byte[] bytes = image.EncodeToPNG();
        Destroy(image);
        File.WriteAllBytes(file, bytes);
        count++;
    }
}
