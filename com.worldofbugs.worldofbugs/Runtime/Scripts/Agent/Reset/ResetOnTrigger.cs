using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace WorldOfBugs {

    public class ResetOnTrigger : MonoBehaviour, IReset {

        public Collider[] Triggers;
        private bool _shouldReset = false;

        public bool ShouldReset(GameObject agent) {
            return _shouldReset && enabled;
        }

        private void OnTriggerEnter(Collider trigger) {
            _shouldReset = Array.Exists(Triggers, x => trigger.Equals(x));
        }

        public void Reset() {
            _shouldReset = false;
        }

        public void OnEnable() {} // allow enabled/disable from inspector.
        public void OnDisable() {}
    }
}
