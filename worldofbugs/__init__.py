#!/usr/bin/env python
# -*- coding: utf-8 -*-
"""
    World of Bugs is a platform for Automated Bug Detection Research, see [Home](../index.md) for details.
"""
__author__ = "Benedict Wilkins"
__email__ = "benrjw@gmail.com"
__status__ = "Development"

from . import dataset, environment, utils
from .environment import all_registered, make

__all__ = ("environment", "utils", "dataset", "make", "all_registered")


def _register_entry_point():  # entry point hook for openai gym
    """Entry point for OpenAI gym."""
    from gym.envs import register

    from .utils import BuildResolver, Logger

    for build in BuildResolver.builds:
        Logger.debug(f"FOUND BUILD: 'WOB/{build.env_id}' at '{build.path}'")
        register(
            id=build.env_id,
            entry_point="worldofbugs:make",
            kwargs=dict(env_id=build.env_id),
        )
