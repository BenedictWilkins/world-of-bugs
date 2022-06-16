#!/usr/bin/env python
# -*- coding: utf-8 -*-
"""
    Some useful utilities for creating/registering new WOB environment builds.
"""
__author__ = "Benedict Wilkins"
__email__ = "benrjw@gmail.com"
__status__ = "Development"


import glob
import os
import pathlib
from typing import List

from loguru import logger as Logger

__all__ = (
    "WorldOfBugsException",
    "BuildResolver",
    "Logger",
)

GYM_NAMESPACE = "WOB"  # this is also in setup.py...


class WorldOfBugsException(Exception):
    """WOB specific Exception."""


class PathList:
    """Container for paths, makes use to `pathlib` to resolve relative paths. Used to resolve builds in `UnityBuildResolver`."""

    def __init__(self, *args, add_callback=None):
        for x in args:
            assert isinstance(x, (str, pathlib.Path, pathlib.PurePath))
        self.paths = [pathlib.Path(x).expanduser().resolve().absolute() for x in args]
        self.add_callback = add_callback

    def __getitem__(self, i):
        return self.paths[i]

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
        """Append new path to the current path collection.
        Args:
            x (str): path
        """
        assert isinstance(x, (str, pathlib.Path, pathlib.PurePath))
        x = pathlib.Path(x).expanduser().resolve().absolute()
        self.paths.append(x)
        self.add_callback(x)

    def __str__(self):
        return str([str(x) for x in self.paths])

    def __repr__(self):
        return str(self)


class UnityBuildResolver:
    """Resolves paths for unity environment builds. Default search order:
    ```
        <CWD>
        <WOB-INSTALL-DIR>/builds/
        <USER-DEFINED-DIR>
        ...
    ```
    Additional paths can be added by modifying the `path` variable, for example:

    ``` worldofbugs.utils.BuildResolver.path += "/path/to/builds/" ```

    The first build found in the search will be given by `get_build`.
    """

    DEFAULT_UNITY_BUILDS_DIR = "builds"
    DEFAULT_UNITY_BUILD_EXT = ".x86_64"  # TODO this is only for linux

    def __init__(self):
        self._builds = None
        self._search_paths = PathList(
            str(pathlib.Path(".").resolve()),
            self.default_path,
            add_callback=self._update,
        )

    @property
    def search_paths(self):
        """Get the list of paths that are used to resolve an enviroment build.

        Returns:
            PathList: list of paths
        """
        return self._search_paths

    @search_paths.setter
    def search_paths(self, path):
        self._search_paths = PathList(*path, add_callback=self._update)
        self.update_builds()

    @property
    def default_path(self):
        """Get the default search path for enviroment builds. See also `UnityBuildResolver.DEFAULT_UNITY_BUILDS_DIR`.

        Returns:
            str: default search path.
        """
        path = pathlib.Path(__file__).parent.parent  # package relative...
        path = pathlib.Path(path, UnityBuildResolver.DEFAULT_UNITY_BUILDS_DIR)
        return str(path.resolve())

    @property
    def builds(self) -> List["BuildPath"]:
        """Get a list of avaliable environment builds.

        Returns:
            List[BuildPath]: builds
        """
        if self._builds is None:
            self.update_builds()
        return self._builds

    def get_build(self, env_id):
        """Attempt to get enviroment build from `env_id`.

        Args:
            env_id (str): environment id

        Raises:
            FileNotFoundError: if the build could not be found.

        Returns:
            BuildPath: builds
        """
        if "/" in env_id:  # WOB namespace was given...
            env_id = "/".join(env_id.split("/")[1:])
        candidate_builds = list(self.builds)
        if len(candidate_builds) > 0:
            return candidate_builds[0]  # get the first one... TODO get all of them?
        envs = [build.name for build in self.builds]
        raise FileNotFoundError(
            f"Failed to find environment: {env_id}. Avaliable environments: {envs}"
        )

    def update_builds(self):
        """Update the list of builds given search paths."""
        self._builds = []
        for path in self._search_paths:
            self._builds.extend(
                _get_builds(path, UnityBuildResolver.DEFAULT_UNITY_BUILD_EXT)
            )

    def _update(self, path):
        self._builds.extend(
            _get_builds(path, UnityBuildResolver.DEFAULT_UNITY_BUILD_EXT)
        )


class BuildPath:
    """
    Utility class that represents a WOB environment build path.
    """

    def __init__(self, path):
        self._path = pathlib.Path(path)

    @property
    def path(self):
        """Get build path as a string.
        Returns:
            str: build path
        """
        return str(self._path)

    @property
    def version(self):
        """Get environment version (<namespace>/<name>_<version>)
        Returns:
            str: environment version
        """
        return self.env_id.rsplit("_", maxsplit=1)[-1]

    @property
    def name(self):
        """Get environment name (<namespace>/<name>_<version>)
        Returns:
            str: environment name
        """
        return self.env_id.split("_", maxsplit=1)[0]

    @property
    def env_id(self):
        """Get environment id (<namespace>/<name>_<version>)
        Returns:
            str: environment id
        """
        return str(self._path.stem)

    def __repr__(self):
        return self.path

    def __str__(self):
        return self.__repr__()


def _get_builds(path, ext):
    environments = []
    Logger.debug(f"Seaching: {path} ...")
    for p in glob.glob(os.path.join(path, "*"), recursive=False):
        p = pathlib.Path(p)
        if p.is_dir():
            # Logger.debug(f"Searching directory: {p} ...")
            p = pathlib.Path(p, f"*{ext}")
            unity_build = glob.glob(str(p))
            if len(unity_build) > 0:
                for env in unity_build:
                    environments.append(BuildPath(env))
    return environments


BuildResolver = UnityBuildResolver()
