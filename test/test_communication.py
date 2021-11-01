
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
env.set_player_behaviour("MLAgents")
env.reset()

for i in range(100):
    action = env.action_space.sample()
    state, *_ = env.step(action)  # Move the simulation forward
   
    assert int(state['InfoSensor'][0]) == action
    print(state['BugMask'].shape, state['Camera'].shape)
    
    #plt.pause(0.001)
    #plt.clf()
    #plt.imshow(np.concatenate([state['BugMask'], state['Camera']], axis=1))
   
env.close()