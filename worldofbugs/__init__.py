#!/usr/bin/env python
# -*- coding: utf-8 -*-
"""
    World of Bugs is a platform for Automated Bug Detection Research, see [Home](../index.md) for details.
"""
__author__ = "Benedict Wilkins"
__email__ = "benrjw@gmail.com"
__status__ = "Development"

def _register_entry_point(): # entry point hook for openai gym
    """ Entry point for OpenAI gym. """
    from .utils import BuildResolver
    from gym.envs import register
    # print("REGISTER ENTRY POINT")
    for build in BuildResolver.builds:
        print(build)
    #register(id="World-v1", entry_point="worldofbugs.utils:make", kwargs=dict(env_id = "World-v1"))

from . import environment
from . import utils
from . import dataset

from .environment import make

