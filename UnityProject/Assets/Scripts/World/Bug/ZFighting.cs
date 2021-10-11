using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZFighting : Bug {  

    public override void Enable() {
        gameObject.SetActive(true);
    }

    public override void Disable() {
        gameObject.SetActive(false);
    }
}
