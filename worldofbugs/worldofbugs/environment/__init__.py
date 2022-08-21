#!/usr/bin/env python
# -*- coding: utf-8 -*-
"""
   The world of bugs environment API. For most usecases #make is sufficient.
"""
__author__ = "Benedict Wilkins"
__email__ = "benrjw@gmail.com"
__status__ = "Development"

from ._action import *
from ._base import *
from ._environment import *
from ._observation import *

ENVIRONMENT_NAMESPACE = "WOB"

__all__ = ("make",)


def all_registered():
    """Find all worldofbugs environments that have been registered with gym.
    Returns:
        List[EnvSpec]: environments
    """
    from gym.envs import registry

    return [x for x in registry.all() if x.namespace == ENVIRONMENT_NAMESPACE]


def make(
    env_id: str,
    worker: int = 0,
    time_scale: float = 1.0,
    log_folder: str = None,
    debug: bool = True,
):
    """Make a `worldofbugs` environment. This is different from the default `gym.make` function as it hooks directly into the underlying Unity environments. This allows one to connect to the a Unity Editor as well as any `worldofbugs` build that may or may not be registered with `gym`. Any builds that exist in the default builds directory for the `worldofbugs` package may be loaded using `gym.make`.

    Args:
       env_id (str): unique environment ID, follows the `gym` format e.g. `WOB/World-v1`. A value of `None` will attempt to connect to a Unity Editor instance.
       worker (int, optional): Worker process to run the environment. This allows multiple environments to run in parallel. Defaults to 0.
       time_scale (float, optional): Simulation speed. WARNING: this can break the game physics, only change if you know what you are doing. Defaults to 1.0.
       log_folder (str, optional): directoy to log to. Defaults to None.
       debug (bool, optional): Get logging messages from unity program and write them to Python stdout. Defaults to True.

    Returns:
       gym.Env: the `worldofbugs` environment.
    """
    from ..utils import BuildResolver

    if env_id is not None:
        build = BuildResolver.get_build(env_id)
        ex_path = build.path
    else:
        ex_path = None  # USE THE UNITY EDITOR
    env = UnityEnvironment(
        file_name=ex_path,
        worker_id=worker,
        log_folder=log_folder,
        display_height=84,
        display_width=84,
        quality_level=1,
        time_scale=time_scale,
        debug=debug,
    )
    env = WOBEnvironment(env)
    return env
