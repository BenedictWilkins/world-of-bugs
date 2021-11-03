from worldofbugs.dataset import dataset, load
from worldofbugs.utils import get_unity_environments, make

import pygame
import numpy as np

#env = make("World-v1")
#env.set_player_behaviour("NavMesh")
BUG_LABELS = ["TextureMissing"]

#dataset("World-v1", n_episodes=100, max_episode_length=1000, workers=20)
# dataset("World-v1", n_episodes=1, max_episode_length=1000, workers=1, bugs=["TextureMissing"])

"""
for i in range(10000):
    action = env.action_space.sample()
    state, *_ = env.step(action)  # Move the simulation forward
    #assert int(state['InfoSensor'][0]) == action
    #print(state['BugMask'].shape, state['Camera'].shape)
    env.render()

env.close()
"""
