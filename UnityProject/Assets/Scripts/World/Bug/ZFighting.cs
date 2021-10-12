using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ZFighting : Bug {  

    public override void Enable() {
        gameObject.SetActive(true);
    }

    public override void Disable() {
        gameObject.SetActive(false);
    }

    public override bool InView(Camera camera) { 
        // shouldnt call this too often its a bit expensive? 
        BugTag tag = gameObject.GetComponent<BugTag>(); 
        int[] mask = BugMask.Instance.Mask(camera); 
        // Compare the mask with my bug type...
        bool result = mask.Contains((int) tag.bugType);
        return result;
    }
}
