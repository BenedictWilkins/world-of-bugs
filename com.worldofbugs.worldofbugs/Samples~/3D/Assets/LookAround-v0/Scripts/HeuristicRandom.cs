using System.Linq;
using UnityEngine;

using Unity.MLAgents.Actuators;
using WorldOfBugs;

namespace WorldOfBugs.LookingAround {


    public class HeuristicRandom : HeuristicComponent {



        protected Vector3 _lookat;
        protected Vector3 LookAt {
            get { return _lookat; }
            set { _lookat = value;}
        }

        void Awake() {
            LookAt = RandomPoint();
            //Debug.Log(LookAt);
        }

        protected float DAngle { get {
            Vector3 position = gameObject.transform.position;
            Vector3 forward = gameObject.transform.forward;
            position.y = 0; // rotation will happen in the xz plane
            forward.y = 0;
            Vector3 direction = (LookAt - position).normalized;
            return Vector3.SignedAngle(forward, direction, Vector3.up);
        }}

        [Tooltip("Chance to do absolutely nothing, this value is corrected using the Horizontal and Vertical Probs.")]
        public float _DoNothingProb = 0.3f;
        public float HorizontalDoNothingProb = 0.2f;
        public float VerticalDoNothingProb = 0.2f;
        // the actual doing nothing prob discounting the chance that both vertical and horizontal actions will be 0
        public float DoNothingProb { get { return _DoNothingProb - (HorizontalDoNothingProb * VerticalDoNothingProb); }}

        public override void Heuristic(in ActionBuffers buffer) {
            var _buffer = buffer.ContinuousActions;
            if (Random.Range(0f,1f) < DoNothingProb) {
                _buffer[0] = 0f;
                _buffer[1] = 0f;
                return;
            }

            //Debug.Log(DAngle);

            if (Random.Range(0f,1f) > HorizontalDoNothingProb) {
                while (Mathf.Abs(DAngle) < 1) {
                    LookAt = RandomPoint();
                    Debug.Log(LookAt);
                }
                //Debug.Log(DAngle);
                _buffer[1] = Mathf.Sign(DAngle);
            } else {
                _buffer[1] = 0f;
            }

            if (Random.Range(0f,1f) > HorizontalDoNothingProb) {
                _buffer[0] = Mathf.Sign(Random.Range(-1f,1f));
            } else {
                _buffer[0] = 0f;
            }
            //Debug.Log($"{_buffer[0]},{_buffer[1]}");
        }

        void OnDrawGizmos() {
            if (enabled) {
                Gizmos.color = Color.red;
                Gizmos.DrawSphere(LookAt, .3f);
            }
        }

        protected Vector3 RandomPoint(){
            float rad = UnityEngine.Random.Range(LookAroundAgent.HMIN, LookAroundAgent.HMAX) * Mathf.Deg2Rad;
            Vector3 position = new Vector3(Mathf.Sin(rad), 0, Mathf.Cos(rad));
            return position; //gameObject.transform.TransformPoint(position);
        }

    }
}
