#!/usr/bin/env python
# -*- coding: utf-8 -*-
"""
    "# MKDOCS IGNORE MODULE"
"""
__author__ = "Benedict Wilkins"
__email__ = "benrjw@gmail.com"
__status__ = "Development"

"""

DEPRECATED: TODO update to match v1.0

import pathlib
import gymu

from .environment import make

class WOBIterator(gymu.iterator):

    def __init__(self, env, policy=None, mode=gymu.mode.sardi, max_length=5000):
        env = make(env)
        if isinstance(policy, str):
            env.set_player_behaviour(policy) # passing
            policy = lambda x : 0 # stub if using a unity policy
            if 'action' in mode.keys():
                assert 'info' in mode.keys() # if using a unity policy, info is required to get action data
                self.__iter__ = self._iter_with_unity_policy(self.__iter__)
        super().__init__(env, policy=policy, mode=mode, max_length=max_length)

    def __iter__(self):
        state, info = self.env.reset()
        done = False
        i = 0
        while not done:
            action = self.policy(state)
            # state, action, reward, next_state, done, info
            next_state, reward, done, next_info = self.env.step(action)
            i += 1
            done = done or i >= self.max_length
            #print(action, next_info.get('info')[0])
            action = next_info.get('info', [action])[0]
            result = (state, action, reward, next_state, done, info) # S_t, A_t, R_{t+1}, S_{t+1}, done_{t+1}, info_{t}
            yield self.mode(*[result[i] for i in self.mode.__index__])
            state = next_state
            info = next_info

def dataset(path, env_id, policy='navmesh', mode=gymu.mode.sardi, max_episode_length=5000, num_episodes=1):
    iter = WOBIterator(env_id, policy=policy, mode=mode, max_length=max_episode_length)
    path = pathlib.Path(path).resolve()
    print(f"Creating dataset at: {str(path)}")
    zfill = max(len(str(num_episodes)), 4)
    for i in range(num_episodes):
        gymu.data.write_episode(iter, pathlib.Path(path, f"ep-{str(i).zfill(zfill)}"))

"""
