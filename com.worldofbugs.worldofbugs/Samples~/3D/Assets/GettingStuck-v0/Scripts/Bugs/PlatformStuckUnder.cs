using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using WorldOfBugs;


public class PlatformStuckUnder : Bug {

    public GameObject Platform;
    public GameObject PitBottom;
    protected Collider UnderCollider;

    public void Awake() {
        foreach (GameObject go in GetLeafChildGameObjectsWithComponent<Renderer>(PitBottom)) {
            Tag(go);
        }
    }
    public override void OnEnable() {
        UnderCollider = Platform.GetChildWithName("Under")?.GetComponent<Collider>();
        UnderCollider.enabled = false;
    }

    public override void OnDisable() {
        if (UnderCollider != null) {
            UnderCollider.enabled = true;
        }
    }

# if UNITY_EDITOR
    public void OnValidate() {
        if(Platform.GetChildWithName("Under") == null) {
            Debug.LogError("Platform requires child GameObject with name 'Under' whose collider prevents the player from moving underneath.", Platform);
        }
    }
# endif

}
