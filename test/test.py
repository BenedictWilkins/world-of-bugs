#!/usr/bin/env python
# -*- coding: utf-8 -*-
""" 
   Created on 07-03-2022
"""
__author__ = "Benedict Wilkins"
__email__ = "benrjw@gmail.com"
__status__ = "Development"

from pprint import pprint
from worldofbugs.environment._base import UnityEnvironment

env = UnityEnvironment()
#env.enable_bug("TextureMissing")
#env.set_agent_behaviour("World1HeuristicNavMesh")
env.reset()

#print(list(env.behavior_specs.keys()))

name = next(iter(env.behavior_specs.keys()))
for i in range(100):
   print(i)
   decision_steps, terminal_steps = env.get_steps(name)
   #env.set_action_for_agent(name, 0, np.array())
   #env.step()