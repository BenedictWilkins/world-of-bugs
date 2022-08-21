using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Policies;

namespace WorldOfBugs {

    public class RejectionSamplingException : WorldOfBugsException {
        public RejectionSamplingException() :
            base("Maximum rejection sampling attempts reached, perhaps the walk radius is too large?") {}
    }

    public class HeuristicNavMesh : HeuristicComponent {


        public static readonly int MAX_REJECTION_SAMPLE_ATTEMPTS = 100;
        [Tooltip("Navmesh surfaces, allows the navmesh to be updated at runtime.")]
        public NavMeshSurface[] surfaces;

        [SerializeField, Tooltip("Radius of the associated agent.")]
        protected float _radius = 1f;
        [SerializeField, Tooltip("Angular Speed of the associated agent.")]
        protected float _angularSpeed = 40f;

        // TODO add this ??
        //[SerializeField, Tooltip("Maximum length of the path to the next point (in corners). Use this to prevent the agent taking very complex paths. ")]
        //protected int _max_path_length = 5;


        public float Radius {
            get {
                float? agent_Radius = GetComponent<AgentDefault>()?.Radius;
                _radius = agent_Radius.GetValueOrDefault();
                return _radius;
            }
        }
        public float AngularSpeed {
            get {
                float? agent_AngularSpeed = GetComponent<AgentDefault>()?.AngularSpeed;
                _angularSpeed = agent_AngularSpeed.GetValueOrDefault();
                return _angularSpeed;
            }
        }

        public float walkRadius = 5f;
        public float rejectionRadius = 1f;
        [RangeAttribute(0f, 1f),
         Tooltip("Probability of taking a random action instead of attempting to move to the target point.")]
        public float mistakeProb = 0.2f;

        [RangeAttribute(0f, 180f),
         Tooltip("Angle +/- from forward direction to sample next target point.")]
        public float sampleAngle = 60;
        [RangeAttribute(1f, 180f),
         Tooltip("Amount to increment the sample angle upon failing to find a valid target point.")]
        public float sampleAngleIncrement = 5;

        //private Unity.MLAgents.Agent player { get { return GetComponent<Unity.MLAgents.Agent>(); }}

        //private Transform transform { get { return gameObject.transform; } }

        protected Vector3 targetPosition;
        protected Vector3 currentPosition {
            get {
                return transform.position;
            }
        }

        protected NavMeshPath path;
        protected Vector3 nextPosition {
            get {
                return path.corners[1];
            }
        }
        protected Vector3 goalPosition {
            get {
                return path.corners[path.corners.Length - 1];
            }
        }

        protected bool IsOnNavMesh {
            get {
                NavMeshHit hit;
                return NavMesh.SamplePosition(currentPosition, out hit, Radius, NavMesh.AllAreas);
            }
        }

        // used to ensure heuristic updates are done properly with a decisionPeriod > 1
        private float time;
        //private float _previous_angle;

        protected string[] action_meanings;
        protected string[] required_actions = new string[] {"none", "forward", "rotateleft", "rotateright"};

        void Awake() {
            time = Time.time;

            if(surfaces.Length == 0) {
                throw new WorldOfBugsException("Built in navigation heuristic needs at least one NavMeshSurface.");
            }

            //_previous_angle = Vector3.SignedAngle(gameObject.transform.forward, Vector3.right, Vector3.up);
        }

        void OnEnable() {
            // REQUIRES ACTIONS none, forward, rotateleft, rotateright
            action_meanings = ActionAttribute.ActionMeanings(
                                  GetComponent<Unity.MLAgents.Agent>()).Select(x => x.Replace("_",
                                          "").ToLower()).ToArray();
        }


        void Start() {
            //Debug.Log("NAVMESH START");
            path = new NavMeshPath();
            Update();
        }

        public override void Heuristic(in ActionBuffers buffer) {
            if(path is null) {
                Start();
            }

            //Debug.Log("Decide...");
            float dt = Time.time - time; // estimated time between heuristic calls...
            time = Time.time;

            if(dt == 0) {
                return; // ??? sometimes this happens...
            }

            // none, forward, rotate_left, rotate_right
            var _buffer = buffer.DiscreteActions;
            _buffer[0] = Array.IndexOf(action_meanings, required_actions[0]); //default do nothing
            //Debug.Log($"Path: {path}");
            //Debug.Log($"Path C: {path.corners}");
            //Debug.Log($"Path Corners: {path.corners.Length}");

            if(path.corners.Length == 0) {
                Debug.LogWarning($"{this} invalid path (empty), on navmesh? {IsOnNavMesh}");
                return; // no path found, just do nothing...?
            }

            if(UnityEngine.Random.value < mistakeProb) {
                // do a random action...
                _buffer[0] = UnityEngine.Random.Range(0, 3);
                return;
            }

            // check should rotate
            Vector3 position = gameObject.transform.position;
            position.y = nextPosition.y; // rotation will happen in the xz plane
            Vector3 direction = (nextPosition - position).normalized;
            float angle = Vector3.SignedAngle(gameObject.transform.forward, direction, Vector3.up);
            float rangle = Mathf.Sign(angle) * dt * AngularSpeed;

            if(Mathf.Abs(angle) > Mathf.Abs(rangle)) {
                // should rotate
                // Debug.Log($"{angle} {rangle} {dt} {Time.deltaTime}");
                if(angle < 0) {
                    _buffer[0] = Array.IndexOf(action_meanings, required_actions[2]); // turn left
                } else if(angle > 0) {
                    _buffer[0] = Array.IndexOf(action_meanings, required_actions[3]);; // turn right
                }
            } else {
                _buffer[0] = Array.IndexOf(action_meanings, required_actions[1]); // move forward
            }

            //Debug.Log(_buffer[0]);
            Debug.DrawLine(gameObject.transform.position, gameObject.transform.position + direction,
                           Color.blue, dt);
            Debug.DrawLine(gameObject.transform.position,
                           gameObject.transform.position + gameObject.transform.forward, Color.green, dt);
        }

        void Update() {
            BuildNavMesh(); // TODO maybe done always rebuild... its fine for small environments but not big ones!
            UpdateNavPath();
        }

        void OnDrawGizmos() {
            if(path != null && path.corners.Length > 0) {
                Gizmos.color = Color.red;
                Gizmos.DrawSphere(nextPosition, .3f);
                Gizmos.DrawSphere(targetPosition, .5f);
            }
        }

        void BuildNavMesh() {
            foreach(NavMeshSurface surface in surfaces) {
                surface.BuildNavMesh();
            }
        }

        protected void UpdateNavPath() {
            if(IsOnNavMesh) {
                //Debug.Log(Vector3.Distance(gameObject.transform.position, goalPosition));
                if(path.corners.Length == 0 ||
                        Vector3.Distance(gameObject.transform.position, goalPosition) < Radius) {
                    targetPosition = nextPoint();
                }

                NavMesh.CalculatePath(currentPosition, targetPosition, NavMesh.AllAreas, path);

                for(int i = 0; i < path.corners.Length - 1; i++) {
                    Debug.DrawLine(path.corners[i], path.corners[i + 1], Color.red);
                }
            } else {
                // the agent is no longer on the nav mesh, stop trying to find new target points! ...??
                //Debug.Log("NOT ON NAVMESH");
                // create a new path, the previous one was probably bad so ditch it.
                path = new NavMeshPath();
            }
        }

        protected Vector3 nextPoint() {
            float range = sampleAngle;

            for(int i = 0; i < MAX_REJECTION_SAMPLE_ATTEMPTS; i ++) {
                NavMeshHit hit;
                float r = Mathf.Min(range + i * sampleAngleIncrement, 180f);
                Vector3 rand = RandomSegmentPoint(UnityEngine.Random.Range(0, walkRadius),
                                                  UnityEngine.Random.Range(-r, r));
                rand = gameObject.transform.TransformPoint(rand);

                //Vector3 rand = transform.position + (UnityEngine.Random.insideUnitSphere * walkRadius);
                if(NavMesh.SamplePosition(rand, out hit, rejectionRadius, NavMesh.AllAreas)) {
                    return hit.position; // a suitable point was found
                } // try again...
            }

            throw new RejectionSamplingException();
        }

        protected Vector3 RandomSegmentPoint(float radius, float angle) {
            float rad = angle * Mathf.Deg2Rad;
            Vector3 position = new Vector3(Mathf.Sin(rad), 0, Mathf.Cos(rad));
            return position * radius;
        }
    }
}
