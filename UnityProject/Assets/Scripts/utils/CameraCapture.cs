using System.IO;
using UnityEngine;
    
public class CameraCapture : MonoBehaviour
{
    public int count;
    public KeyCode screenshotKey = KeyCode.F9;
    public string fileName = "capture";
    protected Camera Camera;
    protected bool shouldCapture = false;

    void Awake() {
        Camera = GetComponent<Camera>();
    }

    void OnPostRender() {
        if (shouldCapture) {
            shouldCapture = false;
            RenderTexture renderTexture = Camera.targetTexture;
            Texture2D image = new Texture2D(renderTexture.width, renderTexture.height);
            image.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
            image.Apply();

            byte[] bytes = image.EncodeToPNG();
            Destroy(image);
            string path = $"{Application.dataPath}/Captures/{fileName}({count}).png";
            File.WriteAllBytes(path, bytes);
            count++;
            Camera.targetTexture = null;
            Debug.Log($"Saved Capture: {path}");
        }
    }
    private void LateUpdate()
    {
        if (Input.GetKeyDown(screenshotKey))
        {
            Camera.targetTexture = RenderTexture.GetTemporary(Screen.width, Screen.height, 16);
            shouldCapture = true;
        }
    }   
}
