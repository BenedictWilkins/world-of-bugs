#!/usr/bin/env python
# -*- coding: utf-8 -*-
""" 
   Created on 07-03-2022
"""
__author__ = "Benedict Wilkins"
__email__ = "benrjw@gmail.com"
__status__ = "Development"

from pprint import pprint
import time
from worldofbugs.environment._base import UnityEnvironment


env = UnityEnvironment()
env.enable_bug("TextureMissing")
env.reset()
print(list(env.behavior_specs.keys()))

#name = next(iter(env.behavior_specs.keys()))
#pprint(name)


#decision_steps, terminal_steps = env.get_steps(name)