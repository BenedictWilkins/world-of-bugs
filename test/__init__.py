from worldofbugs.dataset import dataset, load
from worldofbugs.utils import get_unity_environments, make

import numpy as np
import time

#env = make("World-v1")
#
BUG_LABELS = [#"GeometryClipping", "BoundaryHole", 
"TextureMissing", "ZFighting", 
"ZClipping", "CameraClipping", 
"BlackScreen", "TextureCorrupt", 
"ScreenTear", "GeometryCorruption", 
]

BUGS = ["BoundaryHole"]

#dataset("World-v1", "NavMesh", n_episodes=2, max_episode_length=1000, workers=1, append=0)
#dataset("World-v1", "NavMesh", n_episodes=30, max_episode_length=5000, workers=2, append=1)

for i, bug in enumerate(BUGS, 1):
    print("*********************************************************************")
    print(bug)
    #dataset("World-v1", "NavMesh", n_episodes=10, max_episode_length=5000, workers=2, bugs=[bug], append=0)
    dataset("World-v1", "NavMesh", n_episodes=2, max_episode_length=5000, workers=2, bugs=[bug], append=0)
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
