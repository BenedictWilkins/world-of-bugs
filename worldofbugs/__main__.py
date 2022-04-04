#!/usr/bin/env python
# -*- coding: utf-8 -*-
"""
   Try running `worldofbugs` with its default settings. Requires a build (e.g. `WoB/World-v1`) which can be found [here](https://github.com/BenedictWilkins/world-of-bugs/releases). See [GettingStarted](../GettingStarted/index.md) for details.
"""
__author__ = "Benedict Wilkins"
__email__ = "benrjw@gmail.com"
__status__ = "Development"

import worldofbugs

# if you have unity open 
# env = worldofbugs.make(None)

# add downloaded builds to path
#worldofbugs.utils.BuildResolver.path += "~/Downloads/builds/"
print(worldofbugs.utils.BuildResolver.path)     # list all search paths
print(worldofbugs.utils.BuildResolver.builds)   # list all avaliable environments
# if you have built World-v1 or downloaded a build
env = worldofbugs.make("WOB/World-v1") 
env.set_agent_behaviour("Blah")

env.reset()
for i in range(100):
   state, *_ = env.step(env.action_space.sample())
   print(state.shape)
   env.render() # requires pygame
