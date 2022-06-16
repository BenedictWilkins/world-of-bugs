using System;
using System.IO;
using UnityEngine;

public class CameraCapture : MonoBehaviour {

    public bool capture = false;
    public string path = "~/.krate/WorldOfBugs";

    protected string _path;
    protected Camera Camera;
    protected int count;

    void Awake() {
        Camera = GetComponent<Camera>();
        _path = path + "/"  + DateTime.Now.ToString("yyyy-mm-dd-hh-mm-ss");
        Debug.Log(_path);
        Directory.CreateDirectory(_path);
    }

    void OnPostRender() {
        if (capture) {
            RenderTexture renderTexture = Camera.targetTexture;
            Texture2D image = new Texture2D(renderTexture.width, renderTexture.height);
            image.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
            image.Apply();

            byte[] bytes = image.EncodeToPNG();
            Destroy(image);
            //string path = $"{Application.dataPath}/Captures/{fileName}({count}).png";
            string _count = count.ToString("000000");
            File.WriteAllBytes($"{_path}/Camera-{_count}.png", bytes);
            count++;
            //Debug.Log($"Saved Capture: {_path}");
        }
    }
}
