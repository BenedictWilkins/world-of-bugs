from worldofbugs.dataset import dataset, load
from worldofbugs.utils import get_unity_environments, make

import pygame
import numpy as np

#env = make("World-v1")
#env.set_player_behaviour("NavMesh")
#dataset("World-v1", n_episodes=2, max_episode_length=100, workers=1)

for ep in load("World-v1"):
    print(ep)


"""
for i in range(10000):
    action = env.action_space.sample()
    state, *_ = env.step(action)  # Move the simulation forward
    #assert int(state['InfoSensor'][0]) == action
    #print(state['BugMask'].shape, state['Camera'].shape)
    env.render()

env.close()
"""
