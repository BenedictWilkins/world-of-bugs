﻿using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using NaughtyAttributes;

namespace WorldOfBugs {

    public class GeometryClipping : Bug {

        [Tooltip("Collection of game objects to select colliders from, only leaf objects will be affected.")]
        public GameObject Scene;
        [Tooltip("Number of GameObject Colliders to disable, -1 takes all.")]
        public int n;

        private static System.Random _random = new System.Random();
        private GameObject[] _gos; // disable collider on this game object

        public override void OnEnable() {
            // TODO the bugType should be used to render the backsides of specific objects this color -- rather than rely on the global backside...?
            _gos = GetLeafChildGameObjectsWithComponent<Collider>(Scene, n);

            foreach(GameObject go in _gos) {
                foreach(Collider collider in go.GetComponents<Collider>()) {
                    collider.enabled = false;
                }
            }
        }

        public override void OnDisable() {
            if(_gos != null) {
                foreach(GameObject go in _gos) {
                    if(go != null) {
                        foreach(Collider collider in go.GetComponents<Collider>()) {
                            collider.enabled =
                                true; // TODO should probably save the state of collider on disable...
                        }
                    }
                }
            }
        }

#if UNITY_EDITOR

        public bool Debug;
        [ShowIf("Debug")]
        public Color DebugColour = new Color(1, 1, 1, 1);

        public void OnDrawGizmos() {
            if(_gos != null && Debug) {
                foreach(GameObject go in _gos) {
                    MeshFilter[] mfs = go.GetComponents<MeshFilter>();

                    foreach(MeshFilter mf in mfs) {
                        Gizmos.DrawWireMesh(mf.mesh, 0, go.transform.position, go.transform.rotation,
                                            go.transform.localScale);
                    }
                }
            }
        }

#endif


    }
}
