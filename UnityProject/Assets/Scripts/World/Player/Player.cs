using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Policies;

public class Player : MonoBehaviour {

    [NotNull, Tooltip("Camera that is used to create the bug mask, this camera should use the ShowBug shader and a RenderTexture.")]
    public Camera bugMaskCamera; // camera used to detect bugs
    [NotNull, Tooltip("Camera that is used to create a view of the world.")]
    public Camera mainCamera;

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
    public int DecisionPeriod = 5;
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
        script.gameObject.SetActive(true);
        script.MaxStep = MaxStep;
        script.gameObject.GetComponent<DecisionRequester>().DecisionPeriod = DecisionPeriod;       
        if (behaviour != null) {
            behaviour.gameObject.SetActive(false);
        }
        behaviour = script;
    }

    public void Reset() {
        // end the episode?
    }
}
