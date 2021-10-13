using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlackScreen : Bug {

   
    public Color color = new Color(0f,0f,0f,1f);
    public Vector2Int range = new Vector2Int(5, 50);

    protected GameObject canvas;
    protected bool _enabled = false;
    protected int lastFrame = 0;
    protected int toWait = 0;


    void Awake() { 
        canvas = transform.GetChild(0).gameObject;
    }

    public override void Enable() {
        _enabled = true;
    }

    public override void Disable() {
        _enabled = false;
    }

    void On() {
        color.a = 1;
        canvas.GetComponent<Image>().color = color;
    }

    void Off() {
        color.a = 0;
        canvas.GetComponent<Image>().color = color;
    }

    void toggle() {
        color.a = Mathf.Abs(1 - color.a);
        canvas.GetComponent<Image>().color = color;
    }

    public override bool InView(Camera camera) { 
        return false;
    }

    void Update() { 
        if (_enabled) {
            Debug.Log($"??? {Time.frameCount} {lastFrame} {toWait}");
            if (Time.frameCount - lastFrame > toWait) {
                
                toWait = UnityEngine.Random.Range(range.x, range.y);
                lastFrame = Time.frameCount;
                toggle();
            }
        }
    }
}
