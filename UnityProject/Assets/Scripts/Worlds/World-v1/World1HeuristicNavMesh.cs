using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Policies;

namespace WorldOfBugs {
    
    public class RejectionSamplingException : WorldOfBugsException {
        public RejectionSamplingException() : base("Maximum rejection sampling attempts reached, perhaps the walk radius is too large?") {}
    }
    
    public class World1HeuristicNavMesh : PolicyComponent {
        
        public static readonly int MAX_REJECTION_SAMPLE_ATTEMPTS = 100;
        [Tooltip("Navmesh surfaces, allows the navmesh to be updated at runtime.")]
        public NavMeshSurface[] surfaces;

        public float walkRadius = 5f;
        public float rejectionRadius = 1f;
        [RangeAttribute(0f,1f), Tooltip("Probability of taking a random action instead of attempting to move to the target point.")]
        public float mistakeProb = 0.2f;

        [RangeAttribute(0f,180f), Tooltip("Angle +/- from forward direction to sample next target point.")]
        public float sampleAngle = 60;
        [RangeAttribute(1f, 180f), Tooltip("Amount to increment the sample angle upon failing to find a valid target point.")]
        public float sampleAngleIncrement = 5; 
        
        private World1Agent player { get { return GetComponent<World1Agent>(); }}

        //private Transform transform { get { return gameObject.transform; } }

        private Vector3 targetPosition;
        private Vector3 currentPosition { get { return transform.position; }}

        private NavMeshPath path;
        private Vector3 nextPosition { get { return path.corners[1]; }}
        private Vector3 goalPosition { get { return path.corners[path.corners.Length-1]; }}
        private bool onNavMesh { get { 
            NavMeshHit hit;
            return NavMesh.SamplePosition(currentPosition, out hit, player.Radius, NavMesh.AllAreas);
        }}

        // used to ensure heuristic updates are done properly with a decisionPeriod > 1
        private float time; 
        public override bool isHeuristic { get { return true; }}

        void Awake() {
            time = Time.time;
            if (surfaces.Length == 0) {
                throw new WorldOfBugsException("Built in navigation heuristic needs at least one NavMeshSurface.");
            }
        }

        void Start() {
            //Debug.Log("NAVMESH START");
            path = new NavMeshPath();
            Update();
        }
        
        public override void Heuristic(in ActionBuffers buffer) { 
            if (path is null) {
                Start(); 
            }
            //Debug.Log("Decide...");
            float dt = Time.time - time; // estimated time between heuristic calls...
            time = Time.time;
            
            // none, forward, rotate_left, rotate_right
            var _buffer = buffer.DiscreteActions;
            _buffer[0] = 0; //default do nothing
            //Debug.Log($"Path: {path}");
            //Debug.Log($"Path C: {path.corners}");
            //Debug.Log($"Path Corners: {path.corners.Length}");

            if (path.corners.Length == 0) {
                Debug.LogWarning($"{this} invalid path (empty).");
                return; // no path found, just do nothing...? 
            }

            if (UnityEngine.Random.value < mistakeProb) {
                // do a random action...
                _buffer[0] = UnityEngine.Random.Range(0,3);
                return;
            }
            
            // check should rotate
            Vector3 position = gameObject.transform.position;
            position.y = nextPosition.y; // rotation will happen in the xz plane
            Vector3 direction = (nextPosition - position).normalized;
            float angle = Vector3.SignedAngle(gameObject.transform.forward, direction, Vector3.up);
            float rangle = Mathf.Sign(angle) * dt * player.AngularSpeed; 
            
            if (Mathf.Abs(angle) > Mathf.Abs(rangle)) {
                // should rotate
                if (angle < 0) {
                    _buffer[0] = 2;
                } else if (angle > 0) {
                    _buffer[0] = 3;
                }
            } else {
                // move forward
                _buffer[0] = 1;
            }
            //Debug.Log(_buffer[0]);
            Debug.DrawLine(gameObject.transform.position, gameObject.transform.position + direction, Color.blue, dt);
            Debug.DrawLine(gameObject.transform.position, gameObject.transform.position + gameObject.transform.forward, Color.green, dt);
        }

        void Update() {
            BuildNavMesh(); // TODO maybe done always rebuild... its fine for small environments but not big ones!
            UpdateNavPath();
        }

        void OnDrawGizmos() {
            if (path != null && path.corners.Length > 0) {
                Gizmos.color = Color.red;
                Gizmos.DrawSphere(nextPosition, .3f);
                Gizmos.DrawSphere(targetPosition, .5f);
            }
        }

        void BuildNavMesh() {
            foreach (NavMeshSurface surface in surfaces) {
                surface.BuildNavMesh();
            }
        }

        protected void UpdateNavPath() {
            if (onNavMesh) {
                //Debug.Log(Vector3.Distance(gameObject.transform.position, goalPosition));
                if (path.corners.Length == 0 || Vector3.Distance(gameObject.transform.position, goalPosition) < player.Radius) {
                    targetPosition = nextPoint();
                }
                NavMesh.CalculatePath(currentPosition, targetPosition, NavMesh.AllAreas, path);
                for (int i = 0; i < path.corners.Length - 1; i++) {
                    Debug.DrawLine(path.corners[i], path.corners[i + 1], Color.red);
                }
            } else {
                // the agent is no longer on the nav mesh, stop trying to find new target points!
                //Debug.Log("NOT ON NAVMESH");
                path = new NavMeshPath(); // create a new path, the previous one was probably bad so ditch it.
            }
        }

        protected Vector3 nextPoint() {
            float range = sampleAngle;
            for (int i = 0; i < MAX_REJECTION_SAMPLE_ATTEMPTS; i ++) {
                NavMeshHit hit;
                float r = Mathf.Min(range + i * sampleAngleIncrement, 180f);
                Vector3 rand = RandomSegmentPoint(UnityEngine.Random.Range(0, walkRadius), UnityEngine.Random.Range(-r,r));
                rand = gameObject.transform.TransformPoint(rand);
                //Vector3 rand = transform.position + (UnityEngine.Random.insideUnitSphere * walkRadius);
                if (NavMesh.SamplePosition(rand, out hit, rejectionRadius, NavMesh.AllAreas)) {
                    return hit.position; // a suitable point was found
                } // try again...
            }
            throw new RejectionSamplingException();
        }

        protected Vector3 RandomSegmentPoint(float radius, float angle){
            float rad = angle * Mathf.Deg2Rad;
            Vector3 position = new Vector3(Mathf.Sin(rad), 0, Mathf.Cos(rad));
            return position * radius;
        }
    }
}

