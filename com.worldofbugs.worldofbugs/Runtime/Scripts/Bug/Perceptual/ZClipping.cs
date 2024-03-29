﻿using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace WorldOfBugs {

    public class ZClipping : Bug {

        public static readonly string LAYERTAG = "ZClipping";
        public static readonly string BUGTAG = "ZBug";

        public GameObject level;
        public GameObject mainCamera;

        protected GameObject zCamera;
        protected int layer = -1;

        protected GameObject _fighter;
        protected int _oldlayer = -1;

        void Awake() {
            bugTag = BUGTAG;
            layer = LayerMask.NameToLayer(LAYERTAG);

            if(layer < 0) {
                // if this happens something has gone very wrong.. probably the zfighting mainCamera prefab is also broken...
                throw new KeyNotFoundException(
                    $"\"{ZClipping.LAYERTAG}\" must be a layer, please add it to enable the ZFighting bug.");
            }

            foreach(Camera c in mainCamera.GetComponentsInChildren<Camera>()) {
                HideLayer(c, layer); // hide the zfighting layer if its not already hidden...
            }

            zCamera = new GameObject($"{LAYERTAG}Camera");
            Camera _zCamera = zCamera.AddComponent<Camera>();
            _zCamera.CopyFrom(mainCamera.GetComponent<Camera>());
            zCamera.transform.parent = mainCamera.transform;

            foreach(Camera c in zCamera.GetComponentsInChildren<Camera>()) {
                _MakeZCamera(c);
            }
        }

        protected void _MakeZCamera(Camera camera) {
            // set up zcamera properties
            camera.cullingMask = 0; // hide all layers
            ShowLayer(camera, layer); // show only the zfighting layer
            camera.clearFlags = CameraClearFlags.Depth;
            camera.depth = camera.depth + 1;
        }

        public override void OnEnable() {
            // get children of the given game object (level)
            Transform[] children = level.transform.GetComponentsInChildren<Transform>(true);
            children = Array.FindAll(children,
                                     x => x.GetComponent<Renderer>() != null); // leaf children
            int i = UnityEngine.Random.Range(0, children.Length);
            _fighter = children[i].gameObject;
            _oldlayer = _fighter.layer;
            _fighter.layer = layer;
            Tag(_fighter);
        }

        public override void OnDisable() {
            if(_fighter != null) {
                Material material = _fighter.GetComponent<Renderer>().material;
                Untag(_fighter);
                _fighter.layer = _oldlayer;
                _fighter = null;
            }
        }

        private void ShowLayer(Camera camera, int layer) {
            camera.cullingMask |= 1 << layer;
        }

        private void HideLayer(Camera camera, int layer) {
            camera.cullingMask &=  ~(1 << layer);
        }
    }



}
