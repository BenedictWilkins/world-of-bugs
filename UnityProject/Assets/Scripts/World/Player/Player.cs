using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Policies;

public class Player : MonoBehaviour {

    public float MapBottom = -20; // MOVE THIS SOMEWHERE MORE SUITABLE?

    [SerializeField, NotNull, Tooltip("Camera that is used to create a view of the world.")]
    private Camera _cameraMain;
    [SerializeField, NotNull, Tooltip("Camera that is used to create the bug mask, this camera should use the ShowBug shader and a RenderTexture.")]
    private Camera _cameraBugMask; // camera used to detect bugs
    
    public Camera CameraBugMask { get { return _cameraBugMask; }}
    public Camera CameraMain { get { return _cameraMain; }}

    [SerializeField, Tooltip("Type of behaviour for this agent.")]
    protected BehaviourType behaviourType = BehaviourType.Manual;

    public BehaviourType BehaviourType {
        get { return behaviourType; }
        set { behaviourType = value; UpdateBehaviourType(); }

    }
    protected Behaviour behaviour;

    [Tooltip("Movement speed in units/second")]
    public float movementSpeed = 2;
    [Tooltip("Angular speed in degrees/second")]
    public float angularSpeed = 40;

    public float radius = 0.5f;

    // MLAgents parameters...
    public int DecisionPeriod = 1;
    public int MaxStep = 10000;

    public void Awake() {
        UpdateBehaviourType();
    }

    protected void UpdateBehaviourType() {
        Behaviour script = (Behaviour) null;
        switch(BehaviourType) {
            case (BehaviourType.Manual): {
                script = gameObject.GetComponentInChildren<ManualBehaviour>(true);
                break;
            } 
            case (BehaviourType.NavMesh): {
                script = gameObject.GetComponentInChildren<NavMeshBehaviour>(true);
                break;
            }
            case (BehaviourType.MLAgents): {
                script = gameObject.GetComponentInChildren<MLAgentsBehaviour>(true);
                break;
            }
            default: break;
        }
        if (behaviour != null) {
            behaviour.gameObject.SetActive(false);
        }
        script.gameObject.SetActive(true);
        script.MaxStep = MaxStep;
        script.gameObject.GetComponent<DecisionRequester>().DecisionPeriod = DecisionPeriod;       
        behaviour = script;
    }

    public void Reset() {
        // end the episode?
    }
}
