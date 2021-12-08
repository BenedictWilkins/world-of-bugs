using System;
using UnityEngine;

public abstract class Bug : MonoBehaviour {

    public abstract bool InView(Camera camera); // is the bug inview of the given camera
    
    public static bool GameObjectInView(GameObject go, Camera camera) {
        //Plane[] planes = GeometryUtility.CalculateFrustumPlanes(camera);
        //bool inview = GeometryUtility.TestPlanesAABB(planes , go.GetComponent<Collider>().bounds);
        // TODO need to check for occulsions....
        // TODO use the render texture!?

        // DEBUG MODE? 
        //Debug.Log(camera);
        //if (inview) {
        //    go.GetComponent<Renderer>().material.color = new Color(1f,0f,0f);
        //} else {
        //    go.GetComponent<Renderer>().material.color = new Color(1f,1f,1f);
        //}
        return false;
    }

    public abstract void OnEnable();
    public abstract void OnDisable();

   
}


