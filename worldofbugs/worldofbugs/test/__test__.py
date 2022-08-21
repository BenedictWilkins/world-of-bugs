#!/usr/bin/env python
# -*- coding: utf-8 -*-
"""
"""
__author__ = "Benedict Wilkins"
__email__ = "benrjw@gmail.com"
__status__ = "Development"

import worldofbugs

worldofbugs.utils.BuildResolver.search_paths += (
    "com.worldofbugs.worldofbugs/Samples~/3D/Builds"
)

env = worldofbugs.make("WOB/Maze-v0", debug=False)
# env = worldofbugs.make(None, debug=False)

env.reset()
env.set_agent_behaviour("HeuristicNavMesh")
for i in range(1000):
    state, reward, done, info = env.step(0)
    print(info["Position"], reward, done)
    env.render()

    print(state.min(), state.max(), state.shape)


# import worldofbugs

# if you have unity open
# env = worldofbugs.make("WOB/world-v1", debug=True)

# add downloaded builds to path
#
# print(worldofbugs.utils.BuildResolver.path)     # list all search paths
# print(worldofbugs.utils.BuildResolver.builds)   # list all avaliable environments
# if you have built World-v1 or downloaded a build

# env = worldofbugs.make("WOB/World-v1", debug=False)
# env.set_agent_behaviour("HeuristicManual")

# env.enable_bug("PlatformStuck")
# env.enable_bug("PlatformStuckUnder")
"""
env.reset()

for i in range(1000):
    action = env.action_space.sample() * 2
    state, reward, done, info = env.step()

    print(action, info["Action"], info["Rotation"])
    env.render()  # requires pygame"""
