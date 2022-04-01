using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Policies;

namespace WorldOfBugs { 

    public abstract class Agent : Unity.MLAgents.Agent { 

        public abstract void UsePolicy(PolicyComponent policy);
    
    }
}