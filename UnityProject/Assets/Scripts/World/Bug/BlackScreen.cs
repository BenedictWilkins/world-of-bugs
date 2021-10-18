using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlackScreen : Bug {

    public Canvas canvas;
    public Color color = new Color(0f,0f,0f,1f);
    public Vector2Int range = new Vector2Int(5, 50);

    protected Image image;
    protected bool _enabled = false;
    protected int lastFrame = 0;
    protected int toWait = 0;


    void Awake() { 
        canvas.gameObject.AddComponent<Image>();
        image = canvas.gameObject.GetComponent<Image>();
        GetComponent<BugTag>().TagCanvas(canvas.gameObject);
        Off(); // make the image transparent
    }

    public void OnEnable() {
        _enabled = true;
    }

    public void OnDisable() {
        _enabled = false;
    }

    void On() {
        color.a = 1;
        image.color = color;
    }

    void Off() {
        color.a = 0;
        image.color = color;
    }

    void toggle() {
        color.a = Mathf.Abs(1 - color.a);
        image.color = color;
    }

    public override bool InView(Camera camera) { 
        return false;
    }

    void Update() { 
        if (_enabled) {

            if (Time.frameCount - lastFrame > toWait) {
                
                toWait = UnityEngine.Random.Range(range.x, range.y);
                lastFrame = Time.frameCount;
                toggle();
            }
        }
    }
}
