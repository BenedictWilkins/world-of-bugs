#!/usr/bin/env python
# -*- coding: utf-8 -*-
"""
    Tests for worldofbugs.environment._action
"""
__author__ = "Benedict Wilkins"
__email__ = "benrjw@gmail.com"
__status__ = "Development"

import unittest
from types import SimpleNamespace

import numpy as np
from mlagents_envs.base_env import ActionSpec

from worldofbugs.environment import ActionHandler

__all__ = ("BuildResolverTest",)


class TestAction(unittest.TestCase):
    def test_action_handler_discrete(self):
        action_handler = ActionHandler(ActionSpec(0, (2, 2)))
        action = 0
        action_t = action_handler.get_action_tuple(action)
        assert action_t.discrete[0][0] == action

    def test_action_handler_continous(self):
        spec = ActionSpec(1, tuple())
        action = 3.14
        action_handler = ActionHandler(spec)
        action_t = action_handler.get_action_tuple(action)
        self.assertAlmostEqual(action_t.continuous[0][0], action, places=3)
