#!/usr/bin/env python
# -*- coding: utf-8 -*-
"""
   Created on 16-06-2022
"""
__author__ = "Benedict Wilkins"
__email__ = "benrjw@gmail.com"
__status__ = "Development"

import unittest

import gym
from loguru import logger

import worldofbugs
from worldofbugs.utils import BuildResolver


class Test(unittest.TestCase):
    def test_build_resolver(self):
        builds = BuildResolver.builds
        logger.debug(f"FOUND BUILDS: {builds}")


if __name__ == "__main__":
    unittest.main()

"""
import worldofbugs

# if you have unity open
env = worldofbugs.make(None, debug=True)

# add downloaded builds to path
# worldofbugs.utils.BuildResolver.path += "~/Downloads/builds/"
# print(worldofbugs.utils.BuildResolver.path)     # list all search paths
# print(worldofbugs.utils.BuildResolver.builds)   # list all avaliable environments
# if you have built World-v1 or downloaded a build

# env = worldofbugs.make("WOB/World-v1", debug=False)
# env.set_agent_behaviour("HeuristicManual")

# env.enable_bug("PlatformStuck")
# env.enable_bug("PlatformStuckUnder")

env.reset()

for i in range(1000):
    action = env.action_space.sample() * 2
    state, reward, done, info = env.step()

    print(action, info["Action"], info["Rotation"])
    env.render()  # requires pygame"""
