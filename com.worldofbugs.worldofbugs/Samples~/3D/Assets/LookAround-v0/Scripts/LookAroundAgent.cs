using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents.Sensors.Reflection;
using WorldOfBugs;
using UnityEditor;


namespace WorldOfBugs.LookingAround {

    /// <summary>
    /// Agent that uses two continuous actions.
    /// </summary>
    public class LookAroundAgent : WorldOfBugs.AgentDefault {
        
        [Observable("Action")]
        public new Vector2 Action { 
            get { 
                var buffer = GetStoredActionBuffers().ContinuousActions;
                return new Vector2(buffer[0], buffer[1]); //; 
            }
        }

        [Observable("Rotation")]
        public new Vector3 Rotation { get { return gameObject.transform.eulerAngles; }}

        [Action]
        public void vertical(float angle) {
            transform.Rotate (new Vector3 (angle, 0, 0)); //up-down
            Vector3 rotation = transform.localEulerAngles;
            // clamp angle, yes this is a bit weird... just dont rotate the agent out of this range :)
            rotation.x = (rotation.x + 180f) % 360f;
            rotation.x = Mathf.Clamp(rotation.x, 135f, 225f);
            rotation.x += 180f;
            transform.localEulerAngles = rotation;
        }

        [Action]
        public void horizontal(float angle) {
            transform.Rotate(new Vector3(0, angle, 0),Space.World); //Left-right

           
            Vector3 rotation = transform.localEulerAngles;
            Debug.Log(rotation);
            //rotation.y = (rotation.y + 180f) % 360f;
            rotation.y = Mathf.Clamp(rotation.y, 90f, 270f);
            //rotation.y += 180f;
            transform.localEulerAngles = rotation;
        }

    }
}