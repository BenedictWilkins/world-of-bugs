using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[ExecuteInEditMode]
public class BugMaskReplacementShader : MonoBehaviour
{
    public Shader ReplacementShader;
    public Color OverDrawColor;

    void OnEnable()
    {
        if (ReplacementShader != null) {
           GetComponent<Camera>().SetReplacementShader(ReplacementShader, "RenderType");
        }     
    }

    void OnDisable()
    {
        GetComponent<Camera>().ResetReplacementShader();
    }
}