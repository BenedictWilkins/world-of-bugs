#!/usr/bin/env python
# -*- coding: utf-8 -*-
"""
    Tests for worldofbugs.environment._action
"""
__author__ = "Benedict Wilkins"
__email__ = "benrjw@gmail.com"
__status__ = "Development"

import unittest

ERROR_MISSING_BUILDS = "Cannot perform precommit tests as no enviroment builds were found. Please check build directories {0} for builds, this could be a problem with BuildResolver."

__all__ = ("BuildResolverTest",)


class TestEnvironment(unittest.TestCase):
    def test_environment(self):
        pass  # TODO
