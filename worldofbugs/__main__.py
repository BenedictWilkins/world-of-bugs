#!/usr/bin/env python
# -*- coding: utf-8 -*-
""" 
   To install dependancies correctly it is best to run pip install -e world-of-bugs after cloning the repository from github.
"""
__author__ = "Benedict Wilkins"
__email__ = "benrjw@gmail.com"
__status__ = "Development"

import worldofbugs

# if you have unity open 
# env = worldofbugs.utils.make(None)

# add downloaded builds to path
# worldofbugs.utils.BuildResolver.path = ["/home/ben/Downloads/builds/"]
print("ENVS", worldofbugs.utils.BuildResolver.builds) # list all avaliable environments
# if you have built World-v1 or downloaded a build
env = worldofbugs.make(None) 

env.set_agent_behaviour("World1HeuristicNavMesh")

env.reset()
for i in range(100):
   env.step(env.action_space.sample())
   env.render() # requires pygame
   #env._renderer.record(f"./tmp/record/{i}.png")

print("NEW EPISODE")
env.set_agent_behaviour("PolicyPython")

env.reset()
for i in range(100):
   env.step(env.action_space.sample())
   env.render() # requires pygame
   #env._renderer.record(f"./tmp/record/{i}.png")
