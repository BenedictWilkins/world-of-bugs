#!/usr/bin/env python
# -*- coding: utf-8 -*-
"""
    Created on 11-10-2021 12:38:21

    [Description]
"""
__author__ = "Benedict Wilkins"
__email__ = "benrjw@gmail.com"
__status__ = "Development"

from mlagents_envs.environment import UnityEnvironment
from typing import Dict, List, Optional, Tuple, Mapping as MappingType
from mlagents_envs.side_channel.side_channel import SideChannel
from mlagents_envs.side_channel.engine_configuration_channel import EngineConfigurationChannel

from .sidechannel import UnityLogChannel, UnityConfigChannel

class BuggedUnityEnvironment(UnityEnvironment):

    def __init__(self, 
            file_name = None,
            worker_id: int = 0,
            base_port: Optional[int] = None,
            seed: int = 0,
            no_graphics: bool = False,
            timeout_wait: int = 60,
            additional_args: Optional[List[str]] = None,
            side_channels: Optional[List[SideChannel]] = [],
            log_folder: Optional[str] = None,
            display_width=84, 
            display_height=84, 
            quality_level=3, 
            time_scale=1.0, 
            debug=True,
            **kwargs
            ):

        engine_channel = EngineConfigurationChannel()
        engine_channel.set_configuration_parameters(width=display_width,height=display_height, quality_level=quality_level,time_scale=time_scale)
        side_channels.append(engine_channel)

        self.config_channel = UnityConfigChannel()
        side_channels.append(self.config_channel)

        if debug:
            log_channel = UnityLogChannel()
            side_channels.append(log_channel)

        super().__init__(file_name = file_name, 
                         worker_id = worker_id,
                         base_port = base_port,
                         seed = seed,
                         no_graphics = no_graphics,
                         timeout_wait = timeout_wait, 
                         additional_args=additional_args,
                         side_channels = side_channels,
                         log_folder = log_folder,
                         **kwargs)

    def enable_bug(self, bug):
        msg = f"{bug}:{True}"
        self.config_channel.write(str(msg))

    def disable_bug(self, bug):
        msg = f"{bug}:{False}"
        self.config_channel.write(str(msg))