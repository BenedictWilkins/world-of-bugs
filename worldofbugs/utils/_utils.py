#!/usr/bin/env python
# -*- coding: utf-8 -*-
"""
    Created on 05-10-2021 14:40:48

    [Description]
"""
__author__ = "Benedict Wilkins"
__email__ = "benrjw@gmail.com"
__status__ = "Development"

GYM_NAMESPACE = "WOB" # this is also in setup.py...

import os
import pathlib
import glob
import logging
Logger = logging.getLogger("worldofbugs")
_formatter = logging.Formatter('%(asctime)s - %(name)s - %(levelname)s - %(message)s')
_channel = logging.StreamHandler()
_channel.setFormatter(_formatter)
_channel.setLevel(logging.DEBUG)
Logger.setLevel(logging.DEBUG)
Logger.addHandler(_channel)

__all__ = ("WorldOfBugsException", "BuildResolver")

class WorldOfBugsException(Exception):
    pass 

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
        self._builds = None 

    @property
    def default_path(self):
        path = pathlib.Path(__file__).parent.parent # package realtive...
        path = pathlib.Path(path, _UnityBuildResolver.DEFAULT_UNITY_BUILDS_DIR)
        return str(path.resolve())

    @property
    def builds(self):
        if self._builds is None:
            builds = []
            for path in self.path:
                builds.extend(_get_builds(path, _UnityBuildResolver.DEFAULT_UNITY_BUILD_EXT))
            self._builds = builds
        return self._builds

    def get_build(self, env_id):
        if "/" in env_id: # WOB namespace was given...
            env_id = "/".join(env_id.split("/")[1:])
    
        candidate_builds = [build for build in self.builds]
      
        
        if len(candidate_builds) > 0:
            return candidate_builds[0] # get the first one...
        envs = [build.name for build in self.builds]
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
    Logger.debug(f"SEARCH PATH: {path}")
    for p in glob.glob(os.path.join(path, "**/*"), recursive=False):
        p = pathlib.Path(p)
        if p.is_dir():
            p = pathlib.Path(p, f"*{ext}")
            unity_build = glob.glob(str(p))
            if len(unity_build) > 0:
                for env in unity_build:
                    environments.append(BuildPath(env))
    return environments

BuildResolver = _UnityBuildResolver()
