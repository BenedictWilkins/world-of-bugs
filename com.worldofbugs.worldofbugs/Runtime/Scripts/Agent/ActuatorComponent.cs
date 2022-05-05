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

    public abstract class ActuatorComponent<A> : Unity.MLAgents.Actuators.ActuatorComponent where A : Unity.MLAgents.Agent {
       
        public string Name = "Actuator";
        public PolicyComponent Policy;

        [NotNull]
        public Actions<A> Actions;

        public override ActionSpec ActionSpec { get { return Actions.ActionSpec; } }

    }
}
