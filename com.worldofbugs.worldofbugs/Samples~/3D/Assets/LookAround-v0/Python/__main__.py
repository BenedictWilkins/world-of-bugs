#!/usr/bin/env python
# -*- coding: utf-8 -*-
"""
    Test run of the LookingAround-v0 environment.
"""
__author__ = "Benedict Wilkins"
__email__ = "benrjw@gmail.com"
__status__ = "Development"

import numpy as np

import worldofbugs

# if you have unity open
env = worldofbugs.make(None, debug=True)

env.set_agent_behaviour("HeuristicRandom")
env.reset()

for i in range(1000):
    action = np.sign(env.action_space.sample())  # everything to -1 or +1
    state, reward, done, info = env.step(action)
    print(action, info["Action"], info["Rotation"])
