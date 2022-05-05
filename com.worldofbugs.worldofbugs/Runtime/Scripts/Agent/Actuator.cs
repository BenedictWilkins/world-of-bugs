using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;


using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Policies;

namespace WorldOfBugs {

    public abstract class Actuator<A> : IActuator where A : Unity.MLAgents.Agent {
        
        protected ActuatorComponent<A> _component;
        protected PolicyComponent _policy { get { return _component.Policy; }}

        public string Name { get { return _component.Name; }}
        public ActionSpec ActionSpec { get { return _component.ActionSpec; } }
        
        public Actuator(ActuatorComponent<A> component) { _component = component; }

        public abstract void OnActionReceived(ActionBuffers buffer);
        public abstract void Heuristic(in ActionBuffers buffer);
        public abstract void ResetData();
        public abstract void WriteDiscreteActionMask(IDiscreteActionMask mask);
    }

}