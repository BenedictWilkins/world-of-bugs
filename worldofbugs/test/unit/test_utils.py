#!/usr/bin/env python
# -*- coding: utf-8 -*-
"""
    Tests for worldofbugs.utils
"""
__author__ = "Benedict Wilkins"
__email__ = "benrjw@gmail.com"
__status__ = "Development"

import unittest

from worldofbugs import utils

ERROR_MISSING_BUILDS = "Cannot perform precommit tests as no enviroment builds were found. Please check build directories {0} for builds, this could be a problem with BuildResolver."

__all__ = (
    "BuildResolverTest",
    "WorldOfBugsExceptionTest",
    "BuildPathTest",
    "PathListTest",
)


class BuildResolverTest(unittest.TestCase):
    def test_build_resolver(self):
        builds = utils.BuildResolver.builds
        utils.BuildResolver.search_paths += "."
        assert len(utils.BuildResolver.search_paths) != 0
        if len(builds) == 0:
            raise ValueError(
                ERROR_MISSING_BUILDS.format(utils.BuildResolver.search_paths)
            )
        for buildpath in builds:
            assert buildpath.env_id == f"{buildpath.name}-{buildpath.version}"
            assert buildpath.path.endswith(buildpath.env_id)


class WorldOfBugsExceptionTest(unittest.TestCase):
    def test_exception(self):
        utils.WorldOfBugsException("Something went terribly wrong...")


class BuildPathTest(unittest.TestCase):
    def test_build_path(self):
        buildpath = utils._utils.BuildPath("path/to/build/build-v1")
        assert buildpath.env_id == f"{buildpath.name}-{buildpath.version}"
        assert buildpath.path.endswith(buildpath.env_id)


class PathListTest(unittest.TestCase):
    def test_path_list(self):
        path = "path/to/build/build-v1"
        callback_flag = [False]

        def callback(x):
            callback_flag[0] = True

        pathlist = utils._utils.PathList(path, add_callback=callback)
        pathlist.append(path + "+1")
        pathlist += path + "+2"
        pathlist = pathlist + (path + "+3")
        assert callback_flag[0]  # add_callback is broken in PathList
        assert len(pathlist) == 4
        assert str(pathlist[0]).endswith(path)  # path should be resolved
