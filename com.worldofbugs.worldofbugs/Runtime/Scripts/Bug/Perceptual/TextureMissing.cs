﻿using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
namespace WorldOfBugs {


    public class TextureMissing : Bug {

        public GameObject level;
        public Texture missingTexture;
        public Color color = new Color(1, 1, 1, 1);

        protected GameObject _missing;
        protected Texture _texture;
        protected Color _color;

        public override void OnEnable() {
            // get children of the given game object (level)
            Transform[] children = level.transform.GetComponentsInChildren<Transform>(true);
            children = Array.FindAll(children,
                                     x => x.GetComponent<Renderer>() != null); // leaf children
            int i = UnityEngine.Random.Range(0, children.Length);
            _missing = children[i].gameObject;
            Material material = _missing.GetComponent<Renderer>().material;
            _texture = material.GetTexture("_MainTex");
            _color = material.GetColor("_Color");
            material.SetTexture("_MainTex", missingTexture);
            material.SetColor("_Color", color);
            Tag(_missing);
        }

        public override void OnDisable() {
            if(_missing != null) {
                Material material = _missing.GetComponent<Renderer>().material;
                material.SetTexture("_MainTex", _texture);
                material.SetColor("_Color", _color);
                Untag(_missing);
                _missing = null;
            }
        }
    }
}
