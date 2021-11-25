from worldofbugs.dataset import dataset, load
from worldofbugs.utils import get_unity_environments, make

import pygame
import numpy as np
import time

#env = make("World-v1")
#
BUG_LABELS = ["TextureMissing", "ZFighting", "ZClipping", "CameraClipping", "BlackScreen", "TextureCorrupt", "ScreenTear", "GeometryCorruption"]
BUGS = BUG_LABELS

# dataset("World-v1", n_episodes=100, max_episode_length=1000, workers=20)
# dataset("World-v1", "NavMesh", n_episodes=1, max_episode_length=1000, workers=1, bugs=BUGS)
#dataset("World-v1", "NavMesh", n_episodes=30, max_episode_length=10000, workers=3, bugs=BUGS, append=0)

for bug in BUGS:
    print("*********************************************************************")
    print(bug)
    dataset("World-v1", "NavMesh", n_episodes=1, max_episode_length=1000, workers=1, bugs=[bug], append=1)
    print("*********************************************************************")




"""
for i in range(10000):
    action = env.action_space.sample()
    state, *_ = env.step(action)  # Move the simulation forward
    #assert int(state['InfoSensor'][0]) == action
    #print(state['BugMask'].shape, state['Camera'].shape)
    env.render()

env.close()
"""
