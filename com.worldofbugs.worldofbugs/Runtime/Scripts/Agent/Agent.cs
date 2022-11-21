using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Policies;
using Unity.MLAgents.Sensors.Reflection;

namespace WorldOfBugs {

    public abstract class Agent : Unity.MLAgents.Agent, IReset {

        [SerializeField]
        internal protected HeuristicComponent _heuristic;

        [HideInInspector]
        public IReset[] Resets;
        [HideInInspector]
        public Controller GlobalController;

        public new void OnEnable() {
            // unfortunately there is no way to create and add an actuator directly (see LazyInitialize/Initialize/InitializeActuators in MLAgents.Agent)
            if(GetComponent<ReflectionActuatorComponent>() == null) {
                ReflectionActuatorComponent actuator =
                    gameObject.AddComponent<ReflectionActuatorComponent>();
                actuator.Initialize(this);
            }

            GlobalController = Controller.FindInstanceInScene();
            List<IReset> resets = GetComponents<IReset>().Where(x => !x.Equals(this)).ToList();
            // TODO this needs updating for environments that contain more than one agent. It currently triggers a global reset if the global controller is included.
            resets.Add(GlobalController);
            Resets = resets.ToArray();
            base.OnEnable();
        }

        public void FixedUpdate() {
            // NOTE: the order of this is important.
            // if RequestDecision comes after EndEpisodes, any reward obtained in this "step"
            // will not be sent to python and the "intial state" (after reset) will be sent as the final state...
            RequestDecision();

            foreach(IReset reset in Resets) {
                if(reset.ShouldReset(gameObject)) {
                    EndEpisode();
                    break;
                }
            }
        }

        public virtual void SetHeuristic(HeuristicComponent heuristic) {
            if(_heuristic) {
                _heuristic.enabled = false;
            }

            _heuristic = heuristic;

            if(_heuristic) {
                _heuristic.enabled = true;
            }
        }

        public override sealed void Heuristic(in ActionBuffers buffer) {
            if(_heuristic != null) {
                _heuristic.Heuristic(buffer);
            }
        }

        public override void OnEpisodeBegin() {
            Resets.ToList().ForEach(x => x.Reset());
            Reset();
        }

        public virtual bool ShouldReset(GameObject agent) {
            if(agent != null || !gameObject.Equals(agent)) {
                throw new WorldOfBugsException(
                    $"The supplied GameObject {agent} does not belong to this agent.");
            }

            return Resets.All(x => x.ShouldReset(gameObject));
        }
        /// <summary>
        /// Part of the OnEpisodeBegin template, your agent should reset its internal state here as the episode is over.
        /// </summary>
        /// <see cref="Unity.MLAgents.Agent.OnEpisodeBegin"></see>
        public abstract void Reset();

    }

}
