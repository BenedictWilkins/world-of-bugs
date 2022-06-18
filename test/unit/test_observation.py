#!/usr/bin/env python
# -*- coding: utf-8 -*-
"""
    Tests for worldofbugs.environment._observation
"""
__author__ = "Benedict Wilkins"
__email__ = "benrjw@gmail.com"
__status__ = "Development"

import unittest
from types import SimpleNamespace

from worldofbugs.environment import ObservationHandler

__all__ = ("TestObservation",)


class TestObservation(unittest.TestCase):
    def test_observation_handler(self):
        SHAPE = (100,)
        specs = [SimpleNamespace(name="Agent:Sensor", shape=SHAPE)]
        handler = ObservationHandler(specs)
        # TODO only supports a single agent, the agent ID is removed from the key...? this might be a bad idea for backward compatibility in the future...
        assert handler["Sensor"].shape == SHAPE
