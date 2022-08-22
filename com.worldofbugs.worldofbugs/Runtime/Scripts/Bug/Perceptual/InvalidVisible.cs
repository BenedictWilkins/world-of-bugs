using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace WorldOfBugs {

    /// <summary>
    /// Attach this script to a GameObject and enable it to render the GameObject in the bug mask. This should be used if an object should never be seen by the player. It can also be attached to a Controller Object, in which case such objects should be provided.
    /// </summary>
    //[ExecuteInEditMode] issues with tagging if this is the case...
    public class InvalidVisible : Bug {

        public bool IncludeThis = true;
        public List<GameObject> InvalidGameObjects;

        public override void OnEnable() {
            if(IncludeThis) {
                Tag(gameObject);
            }

            foreach(GameObject go in InvalidGameObjects) {
                Tag(go);
            }
        }

        public override void OnDisable() {
            Untag(gameObject);

            foreach(GameObject go in InvalidGameObjects) {
                Untag(go);
            }
        }
    }
}
