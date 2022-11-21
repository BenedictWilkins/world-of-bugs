using System;
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

        private GameObject[] gameObjects; // disable collider on these game objects

        public override void OnEnable() {
            // TODO the bugType should be used to render the backsides of specific objects this color -- rather than rely on the global backside...?
            gameObjects = GetLeafChildGameObjectsWithComponent<Collider>(Scene, n);

            foreach(GameObject go in gameObjects) {
                foreach(Collider collider in go.GetComponents<Collider>()) {
                    collider.enabled = false;
                }
            }
        }

        public override void OnDisable() {
            if(gameObjects != null) {
                foreach(GameObject go in gameObjects) {
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

        public void OnDrawGizmos() {
            HighlightGameObjects(gameObjects);
        }

#endif


    }
}
