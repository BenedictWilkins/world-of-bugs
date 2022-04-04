#!/usr/bin/env python
# -*- coding: utf-8 -*-
"""
    Side channels are used to communicate data to Unity and are a feature of Unity MLAgents.
    `worldofbug` defines two useful, one for logging, and one for configuration.
"""
__author__ = "Benedict Wilkins"
__email__ = "benrjw@gmail.com"
__status__ = "Development"

from mlagents_envs.side_channel.side_channel import SideChannel, IncomingMessage, OutgoingMessage
import uuid

class UnityLogChannel(SideChannel):
    """ Logging SideChannel, reads log messages from Unity and prints them to stdout."""
    def __init__(self):
        super().__init__(uuid.UUID("c601d1fe-b487-454e-a6b3-b7305d884442")) # share unique channel ID

    def on_message_received(self, msg):
        print(msg.read_string())

class UnityConfigChannel(SideChannel):
    """ Configuration SideChannels, sends config information to a Unity environment. For API details see: [TODO]()"""
    def __init__(self):
        super().__init__(uuid.UUID("621f0a70-4f87-11ea-a6bf-784f4387d1f7"))

    def on_message_received(self, msg):
        raise NotImplementedError("Config Channel received a message... this shouldnt happen!" + msg.read_string())

    def write(self, msg):
        outgoing = OutgoingMessage()
        outgoing.write_string(msg)
        super().queue_message_to_send(outgoing)

