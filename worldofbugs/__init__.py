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
    from .utils import BuildResolver, Logger
    from gym.envs import register, registry
    for build in BuildResolver.builds:
        Logger.debug(f"FOUND BUILD: 'WOB/{build.env_id}' at '{build.path}'")
        register(id=build.env_id, entry_point="worldofbugs:make", kwargs=dict(env_id = build.env_id))

from . import environment
from . import utils
from . import dataset

from .environment import make, all

