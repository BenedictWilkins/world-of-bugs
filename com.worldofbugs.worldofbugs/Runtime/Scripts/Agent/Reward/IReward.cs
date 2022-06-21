using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WorldOfBugs {
    public interface IReward {
        public void Reward(Agent agent);
    }
}
