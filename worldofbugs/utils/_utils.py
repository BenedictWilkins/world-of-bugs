#!/usr/bin/env python
# -*- coding: utf-8 -*-
"""
    Created on 05-10-2021 14:40:48

    [Description]
"""
__author__ = "Benedict Wilkins"
__email__ = "benrjw@gmail.com"
__status__ = "Development"


from dataclasses import dataclass
import os
import pathlib
import glob
import gym


from ..environment import UnityEnvironment, WOBEnvironment

__all__ = ('make', 'BuildResolver')

# use side channels?
# https://github.com/Unity-Technologies/ml-agents/blob/main/docs/Python-API.md#interacting-with-a-unity-environment
def make(env_id, worker=0, display_width=84, display_height=84, quality_level=3, time_scale=1.0, log_folder=None, debug=True): 
    if env_id is not None:
        build = BuildResolver.get_build(env_id)
        ex_path = build.path
        assert build.name == env_id
    else:
        ex_path = None # USE THE UNITY EDITOR
    env = UnityEnvironment(file_name=ex_path, 
                                    worker_id=worker, 
                                    log_folder=log_folder, 
                                    display_height=display_height, 
                                    display_width=display_width, 
                                    quality_level=quality_level, 
                                    time_scale=time_scale, 
                                    debug=debug)
    env = WOBEnvironment(env)
    return env

class _UnityBuildResolver:

    """ Resolves paths for unity environment builds. Default search order:

        Current working directory
        <WOB-INSTALL-DIR>/builds/
        
        Additional paths can be added by modifying the `path` variable, for example:

        ``` worldofbugs.utils.BuildResolver.path += "/path/to/builds/" ```

        The first build found in the search will be given by `get_build`.
    """
    DEFAULT_UNITY_BUILDS_DIR = "builds"
    DEFAULT_UNITY_BUILD_EXT = ".x86_64" # TODO this is only for linux

    def __init__(self):
        self.path = [str(pathlib.Path(".").resolve()), self.default_path]    

    @property
    def default_path(self):
        path = pathlib.Path(__file__).parent.parent # package realtive...
        path = pathlib.Path(path, _UnityBuildResolver.DEFAULT_UNITY_BUILDS_DIR)
        return str(path.resolve())

    def get_builds(self):
        envs = []
        for path in self.path:
            envs.extend(_get_builds(path, _UnityBuildResolver.DEFAULT_UNITY_BUILD_EXT))
        return envs

    def get_build(self, env_id):
        candidate_builds = [build for build in self.get_builds() if env_id == build.name]
        if len(candidate_builds) > 0:
            return candidate_builds[0] # get the first one...
        envs = [build.name for build in self.get_builds()]
        raise FileNotFoundError(f"Failed to find environment: {env_id}. Avaliable environments: {envs}")

class BuildPath:

    def __init__(self, path):
        self._path = pathlib.Path(path)
    
    @property
    def path(self):
        return str(self._path)

    @property
    def version(self):
        pass 

    @property
    def name(self):
        return self._path.stem

    def __repr__(self):
        return self.path
    
def _get_builds(path, ext):
    environments = []
    for p in glob.glob(os.path.join(path, "*")):
        p = pathlib.Path(p)
        if p.is_dir():
            p = pathlib.Path(p, f"*{ext}")
            unity_build = glob.glob(str(p))
            if len(unity_build) > 0:
                for env in unity_build:
                    environments.append(BuildPath(env))
    return environments

BuildResolver = _UnityBuildResolver()
