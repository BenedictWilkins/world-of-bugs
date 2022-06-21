using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace WorldOfBugs {

    public class RewardOnTrigger : MonoBehaviour, IReward {

        /// <summary>
        /// Trigger that the agent may collider with to gain some reward.
        /// </summary>
        public Collider trigger;
        /// <summary>
        /// Amount of reward gained (added to the current total).
        /// </summary>
        public float reward;
        /// <summary>
        /// Time to wait before the agent is again rewarded for colliding with the trigger in seconds.
        /// </summary>
        public float wait = 1;

        private float _rewardedAt = 0;
        private bool _shouldReward = false;

        public void Reward(Agent agent) {
            float _newRewaredAt = Time.time;

            if(_shouldReward && _newRewaredAt - _rewardedAt > wait) {
                agent.AddReward(reward);
                _rewardedAt = Time.time;
                Debug.Log(GetComponent<Agent>().GetCumulativeReward());
            }
        }

        public void OnTriggerEnter(Collider other) {
            if(trigger.Equals(other)) {
                _shouldReward = true;
                Reward(GetComponent<Agent>());
            }
        }

        public void OnTriggerExit(Collider other) {
            if(trigger.Equals(other)) {
                _shouldReward = false;
            }
        }
    }
}
