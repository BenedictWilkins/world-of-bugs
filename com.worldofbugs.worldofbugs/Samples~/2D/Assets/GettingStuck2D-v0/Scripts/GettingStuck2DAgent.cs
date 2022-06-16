using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GettingStuck2DAgent : MonoBehaviour  { //Agent {

    public bool SnapToGrid = false;
    public float Speed = 5f;
    public float CellSize = 1f;
    private Vector3 position;

    private Sprite IdleAnimation;
    private Sprite RunAnimation;

    private SpriteRenderer Renderer;

    // Start is called before the first frame update
    void Awake() {
        position = transform.position;
        //GetComponent<Rigidbody2D>().gravityScale)(0)
        Renderer = GetComponent<SpriteRenderer>();
        Debug.Log(Renderer.sprite.rect);
    }

    // Update is called once per frame
    void Update() {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        Debug.Log($"{h}, {v}");
        position = transform.position;

        if(h != 0f) {
            position += Time.deltaTime * Speed * new Vector3(h, 0, 0);
            Renderer.flipX = !Convert.ToBoolean((h + 1) / 2);
        }

        if(v != 0f) {
            position += Time.deltaTime * Speed * new Vector3(0, v, 0);
        }

        Debug.Log(position);
        transform.position = Vector3.MoveTowards(transform.position, position,
                             Speed * Time.deltaTime);
        //transform.position = new Vector3(position[0] % CellSize, position[1] % CellSize, position[2] % CellSize);
    }
}
