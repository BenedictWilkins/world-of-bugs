using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public abstract class Interactable : MonoBehaviour {

    protected class Interactor {
        public event Action<Collider> interact;
        public void Interact(Collider collider) {
            interact(collider);
        }
    }

    protected static Dictionary<Collider, Interactor> Interactors = new Dictionary<Collider, Interactor>();

    public static void Interact(Collider collider) {
        if (Interactors.ContainsKey(collider)) {
            // Interactors[collider](collider);
        } else {
            throw new KeyNotFoundException($"Invalid interaction, collider {collider} was not recognised, did you forget to register it?");
        }
    }

    public static void Register(Collider collider) {
        Interactors.Add(collider, new Interactor());
    }

    public abstract void OnInteract(Collider collider);

    public void Awake() {
        Collider[] triggers = GetComponents<Collider>();
        if (triggers.Any(x => x.isTrigger)) {
            Debug.LogWarning($"Found Collider(s) but no triggers. This interactable {gameObject} cannot be interacted with");
        }
    }

    public void OnTriggerEnter(Collider other) {
        Debug.Log("TRIGGER ENTER");
        if (Interactors.ContainsKey(other)) {
            Interactors[other].interact += OnInteract;
        }
    }

    public void OnTriggerExit(Collider other) {
        if (Interactors.ContainsKey(other)) {
            Interactors[other].interact -= OnInteract;
        }
    }

}
