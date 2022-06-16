#!/usr/bin/env python
# -*- coding: utf-8 -*-
__author__ = "Benedict Wilkins"
__email__ = "benrjw@gmail.com"
__status__ = "Development"

GYM_NAMESPACE = "WOB" # this is also in setup.py...

import os
import pathlib
import glob
from loguru import logger as Logger

__all__ = ("WorldOfBugsException", "BuildResolver", "Logger")

class WorldOfBugsException(Exception):
    pass 

class _PathList:

    def __init__(self, *args, add_callback=None):
        for x in args:
            assert isinstance(x, (str, pathlib.Path, pathlib.PurePath))
        self.paths = [pathlib.Path(x).expanduser().resolve().absolute() for x in args]
        self.add_callback = add_callback

    def __getitem__(self, i):
        return  self.paths[i]

    def __iter__(self):
        yield from self.paths

    def __add__(self, x):
        assert isinstance(x, (str, pathlib.Path, pathlib.PurePath))
        x = pathlib.Path(x).expanduser().resolve().absolute()
        self.paths += [x]
        return self

    def __iadd__(self, x):
        assert isinstance(x, (str, pathlib.Path, pathlib.PurePath))
        x = pathlib.Path(x).expanduser().resolve().absolute()
        self.paths += [x]
        return self
    
    def append(self, x):
        assert isinstance(x, (str, pathlib.Path, pathlib.PurePath))
        x = pathlib.Path(x).expanduser().resolve().absolute()
        self.paths.append(x)
        self.add_callback(x)

    def __str__(self):
        return str([str(x) for x  in self.paths])
    
    def __repr__(self):
        return str(self)

class _UnityBuildResolver:
    """ Resolves paths for unity environment builds. Default search order:

        Current working directory
        <WOB-INSTALL-DIR>/builds/
        <USER-DEFINED-DIR> ...
        
        Additional paths can be added by modifying the `path` variable, for example:

        ``` worldofbugs.utils.BuildResolver.path += "/path/to/builds/" ```

        The first build found in the search will be given by `get_build`.
    """
    DEFAULT_UNITY_BUILDS_DIR = "builds"
    DEFAULT_UNITY_BUILD_EXT = ".x86_64" # TODO this is only for linux

    def __init__(self):
        self._builds = None
        self._path = _PathList(
            str(pathlib.Path(".").resolve()), 
            self.default_path,
            add_callback=self._update)

    @property
    def path(self):
        return self._path

    @path.setter
    def path(self, path):
        self._path = _PathList(*path, add_callback=self._update)
        self.update_builds()

    @property
    def default_path(self):
        path = pathlib.Path(__file__).parent.parent # package relative...
        path = pathlib.Path(path, _UnityBuildResolver.DEFAULT_UNITY_BUILDS_DIR)
        return str(path.resolve())

    @property
    def builds(self):
        if self._builds is None:
            self.update_builds()
        return self._builds

    def get_build(self, env_id):
        if "/" in env_id: # WOB namespace was given...
            env_id = "/".join(env_id.split("/")[1:])
        candidate_builds = [build for build in self.builds]
        if len(candidate_builds) > 0:
            return candidate_builds[0] # get the first one...
        envs = [build.name for build in self.builds]
        raise FileNotFoundError(f"Failed to find environment: {env_id}. Avaliable environments: {envs}")

    def update_builds(self):
        self._builds = []
        for path in self.path:
            self._builds.extend(_get_builds(path, _UnityBuildResolver.DEFAULT_UNITY_BUILD_EXT))
    
    def _update(self, path):
        self._builds.extend(_get_builds(path, _UnityBuildResolver.DEFAULT_UNITY_BUILD_EXT))


class BuildPath:

    def __init__(self, path):
        self._path = pathlib.Path(path)
    
    @property
    def path(self):
        return str(self._path)

    @property
    def version(self):
        return self.env_id.split("_")[-1]

    @property
    def name(self):
        return self.env_id.split("_")[0]

    @property
    def env_id(self):
        return str(self._path.stem)
        
    def __repr__(self):
        return self.path
    
def _get_builds(path, ext):
    environments = []
    Logger.debug(f"Seaching: {path} ...")
    for p in glob.glob(os.path.join(path, "*"), recursive=False):
        p = pathlib.Path(p)
        if p.is_dir():
            #Logger.debug(f"Searching directory: {p} ...")
            p = pathlib.Path(p, f"*{ext}")
            unity_build = glob.glob(str(p))
            if len(unity_build) > 0:
                for env in unity_build:
                    environments.append(BuildPath(env))
    return environments

BuildResolver = _UnityBuildResolver()
