using System.Collections;
using System.Collections.Generic;
using UnityEngine;


using WorldOfBugs;

namespace WorldOfBugs {

    public class World1Agent : WorldOfBugs.AgentDefault {


        [Action]
        public void none() {} // does nothing

        [Action]
        public void forward() {
            Vector3 movement = gameObject.transform.forward * Time.deltaTime * MovementSpeed;
            gameObject.transform.Translate(movement,
                                           Space.World); // TODO change this to move position?
        }

        [Action]
        public void rotate_left() {
            gameObject.transform.Rotate(new Vector3(0, -1 * Time.deltaTime * AngularSpeed, 0));
        }

        [Action]
        public void rotate_right() {
            gameObject.transform.Rotate(new Vector3(0, 1 * Time.deltaTime * AngularSpeed, 0));
        }
    }
}
