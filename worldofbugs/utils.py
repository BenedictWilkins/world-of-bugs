#!/usr/bin/env python
# -*- coding: utf-8 -*-
"""
    Created on 05-10-2021 14:40:48

    [Description]
"""
__author__ = "Benedict Wilkins"
__email__ = "benrjw@gmail.com"
__status__ = "Development"

import re
import os
import glob
import h5py
import gym
import numpy as np

from mlagents_envs.environment import UnityEnvironment
from gym_unity.envs import UnityToGymWrapper

UNITY_BUILDS_DIR = "builds"
UNITY_BUILD_EXT = ".x86_64"

class ObservationUnwrap(gym.ObservationWrapper):
    def observation(self, obs):
        return obs[0]

def make(env_id, worker=0): 
    env_id, ex_path = _get_unity_environment_info(env_id)
    env = UnityEnvironment(file_name=ex_path, worker_id=worker)
    env = UnityToGymWrapper(env, uint8_visual=True, allow_multiple_obs=True)
    env = ObservationUnwrap(env)
    return env

def get_unity_environments():
    path = os.path.split(__file__)[0]
    path = os.path.join(path, UNITY_BUILDS_DIR)
    path = os.path.abspath(path)
    environments = []
    info = []
    for dir in glob.glob(os.path.join(path, "*")):
        if os.path.isdir(dir):
            dir = os.path.join(dir, "*{0}".format(UNITY_BUILD_EXT))
            unity_build = glob.glob(dir)
            if len(unity_build) > 0:
                for env in unity_build:
                    info.append(env)
                    environments.append(os.path.splitext(os.path.split(env)[-1])[0])
    return environments, info

def _get_unity_environment_info(env_id):
    envs, infos = get_unity_environments()
    try:
        env_indx = envs.index(env_id)
        ex_path = infos[env_indx]
        return env_id, ex_path
    except IndexError:
        raise FileNotFoundError(f"Failed to find environment: {env_id}. Avaliable environments include: {envs}")

if __name__ == "__main__":
    print(get_unity_environments())
    env = make("World-v1")
    state = env.reset()
    print(state.shape)
    for i in range(100):
        action = env.action_space.sample()
        state, *_ = env.step(action)
        print(state.shape)