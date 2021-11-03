
from mlagents_envs.environment import UnityEnvironment

import matplotlib.pyplot as plt
import numpy as np

#plt.ion()
#fig = plt.figure()

from worldofbugs.environment import BuggedUnityEnvironment, BuggedUnityGymEnvironment

# We start the communication with the Unity Editor and pass the string_log side channel as input
env = BuggedUnityEnvironment()
env = BuggedUnityGymEnvironment(env)

# Manual, MLAgents, NavMesh
# env.set_player_behaviour("MLAgents")
env.reset()
env.render()

for i in range(1000):
    action = env.action_space.sample()
    state, *_ = env.step(action)  # Move the simulation forward
    env.render()

env.close()