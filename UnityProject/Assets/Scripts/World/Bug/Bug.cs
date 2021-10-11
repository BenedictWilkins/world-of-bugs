using System;
using UnityEngine;

public abstract class Bug : MonoBehaviour {
    
    // enable the bug, this will have different effects for different kinds of bugs...
    public abstract void Enable();
    public abstract void Disable();
        
}


