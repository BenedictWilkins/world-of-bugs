using System;
using UnityEngine;

namespace WorldOfBugs {

    public class ResetY : MonoBehaviour, IReset {

        public float Y = -10;

        public bool ShouldReset(GameObject agent) {
            return agent.transform.position.y < Y;
        }

        public void Reset() {}
    }


}
