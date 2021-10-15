using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenTear : MonoBehaviour {

    public Material screenTearMat; 

    void Awake() {
    }

    void OnRenderImage(RenderTexture source, RenderTexture destination) {
        Graphics.Blit(source, destination, screenTearMat);
    }
}
