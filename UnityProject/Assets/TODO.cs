using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TOOD : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

        // missing texure bugs -- bug mask compatible (as long as the whole texture is missing)
        // visual artefacts -- can just do this by placing random game objects around, bug mask compatible :) 
        // geometry issues -- use the geometry script from before, bug mask compatible :) 
        // clipping (collision) -- might be more difficult, the mask doesnt quite capture it ! the intersection must be visible...
        // map hole -- see clipping (camera), this can also just be checked by looking at the y coord of the player
        // stuck -- prevent actions from working at a specific position? 
        // freezing/fps issues -- use the old script!
        // Screen tearing -- Hmmm...
        // cant interact with specific object ? (maybe...)
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
