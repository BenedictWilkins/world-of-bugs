#!/usr/bin/env python
# -*- coding: utf-8 -*-
"""
    Created on 05-10-2021 14:40:48

    [Description]
"""
__author__ = "Benedict Wilkins"
__email__ = "benrjw@gmail.com"
__status__ = "Development"


import os
import glob
import gym

from gym_unity.envs import UnityToGymWrapper
from mlagents_envs.environment import UnityEnvironment

from .environment import BuggedUnityEnvironment, BuggedUnityGymEnvironment

UNITY_BUILDS_DIR = "builds"
UNITY_BUILD_EXT = ".x86_64"

class ObservationUnwrap(gym.ObservationWrapper):
    def observation(self, obs):
        return obs[0]


# use side channels?
# https://github.com/Unity-Technologies/ml-agents/blob/main/docs/Python-API.md#interacting-with-a-unity-environment
def make(env_id, worker=0, display_width=84, display_height=84, quality_level=3, time_scale=1.0, log_folder=None, debug=True): 
    if env_id is not None:
        env_id, ex_path = _get_unity_environment_info(env_id)
    else:
        ex_path = None # USE THE UNITY EDITOR
    env = BuggedUnityEnvironment(file_name=ex_path, 
                                    worker_id=worker, 
                                    log_folder=log_folder, 
                                    display_height=display_height, 
                                    display_width=display_width, 
                                    quality_level=quality_level, 
                                    time_scale=time_scale, 
                                    debug=debug)
    env = BuggedUnityGymEnvironment(env)
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