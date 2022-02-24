#!/usr/bin/env python
# -*- coding: utf-8 -*-
""" 
   Created on 24-02-2022
"""
__author__ = "Benedict Wilkins"
__email__ = "benrjw@gmail.com"
__status__ = "Development"

import worldofbugs

env = worldofbugs.utils.make(None)
env.set_player_behaviour('NavMesh') 

env.reset()
for i in range(1000):
   env.step(env.action_space.sample())
   env.render() # requires pygame installed


#worldofbugs.dataset.dataset("./dataset", "World-v1", max_episode_length=10)


