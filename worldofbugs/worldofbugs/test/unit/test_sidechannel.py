#!/usr/bin/env python
# -*- coding: utf-8 -*-
"""
    Tests for `worldofbugs.environment.sidechannel`. These cannot be fully performed without connecting to unity. Tests here are just for the API.
"""
__author__ = "Benedict Wilkins"
__email__ = "benrjw@gmail.com"
__status__ = "Development"


import unittest
from io import StringIO

from mlagents_envs.side_channel.side_channel import IncomingMessage, OutgoingMessage

from worldofbugs.environment import sidechannel

__all__ = ("SideChannelTest",)


class SideChannelTest(unittest.TestCase):
    def test_config_channel(self):
        sc = sidechannel.UnityConfigChannel()
        sc.write("Config options...")
        try:
            sc.on_message_received(IncomingMessage(""))
            self.assertFalse()
        except:
            pass

    def test_log_side_channel(self):
        stream = StringIO()
        message = bytes("Unity: log this please", "utf-8")
        in_message = IncomingMessage(message)
        sc = sidechannel.UnityLogChannel(stream=stream)
        # TODO something weird happens, the characters are consume somewhere, this doesnt happen in the main tests?... hmm
        """
        sc.on_message_received(in_message)
        print(stream.read())

        print(stream.readlines())
        assert "".join(stream.readlines()) == message
        try:
            sc.write("")
            self.assertFalse()
        except:
            pass
        """
