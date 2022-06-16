using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WorldOfBugs;

public class PlatformStuck : Bug {

    private static readonly float STUCK_Y = -0.5f;

    public GameObject Platform;
    //protected Collider UnderCollider;
    protected Platform PlatformScript;
    protected bool BugActive;


    public override void OnEnable() {
        //UnderCollider = Platform.GetChildWithName("Under")?.GetComponent<Collider>();
        //UnderCollider.enabled = false;
        PlatformScript = Platform.GetComponent<Platform>();
    }

    public override void OnDisable() {
        //UnderCollider.enabled = false;
        if(Platform != null) {
            Untag(Platform);
        }

        BugActive = true;
    }

    public void Update() {
        // check if the agent is in the put and cant get out...?
        if(!BugActive && !PlatformScript.enabled) {  // the platform has stopped
            Tag(Platform);
            BugActive = true;
        }
    }

}
