using System;
using UnityEngine;
using UnityEngine.Events;

namespace WorldOfBugs {


    [RequireComponent(typeof(Collider))]
    public class ColliderTrigger : MonoBehaviour {
        
        public Collider[] Triggers;

        public UnityEvent OnEnter;
        public UnityEvent OnExit;

        public void OnEnable() {

        }

        public void OnTriggerEnter(Collider collider) {
            
            if (enabled && Array.Exists(Triggers, x => x == collider)) {
                OnEnter.Invoke();
            }
        }   

        public void OnTriggerExit(Collider collider) {
            if (enabled && Array.Exists(Triggers, x => x == collider)) {
                OnExit.Invoke();
            }
        }

        public static void EnableComponent(Component component) {
            ((Behaviour)component).enabled = true;
        }

        public static void DisableComponent(Component component) {
            ((Behaviour)component).enabled = false;
        }

        public static void EnableGameObject(GameObject gameObject) {
            gameObject.SetActive(true);
        }

        public static void DisableGameObject(GameObject gameObject) {
            gameObject.SetActive(false);
        }


    }

}