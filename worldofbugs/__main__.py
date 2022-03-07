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
# worldofbugs.utils.BuildResolver.path += "/home/ben/Downloads/builds/"
# print(worldofbugs.utils.BuildResolver.get_builds()) # list all avaliable environments

# if you have built World-v1 or downloaded a build
env = worldofbugs.utils.make('World-v1') 

env.reset()
for i in range(1000):
   env.step(env.action_space.sample())
   env.render() # requires pygame

