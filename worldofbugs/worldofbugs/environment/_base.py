"""
    Environment base classes.
"""
__author__ = "Benedict Wilkins"
__email__ = "benrjw@gmail.com"
__status__ = "Development"

from sys import stdout
from typing import List, Optional

import numpy as np
from gym_unity.envs import ActionTuple
from mlagents_envs.environment import UnityEnvironment as MLAgentUnityEnvironment
from mlagents_envs.side_channel.engine_configuration_channel import (
    EngineConfigurationChannel,
)
from mlagents_envs.side_channel.side_channel import SideChannel

from .sidechannel import UnityConfigChannel, UnityLogChannel

__all__ = ("UnityEnvironment",)


class _DevnullStream:
    def write(self, *_):
        pass


class UnityEnvironment(MLAgentUnityEnvironment):
    """
    Wrapped UnityMLAgents environment. Adds some additional functionality with a
    side channel to the game controller in Unity.
    """

    def __init__(
        self,
        file_name=None,
        worker_id: int = 0,
        base_port: Optional[int] = None,
        seed: int = None,
        no_graphics: bool = False,
        timeout_wait: int = 60,
        additional_args: Optional[List[str]] = None,
        side_channels: Optional[List[SideChannel]] = None,
        log_folder: Optional[str] = None,
        display_width=84,
        display_height=84,
        quality_level=3,
        time_scale=1.0,
        debug=True,
        **kwargs,
    ):
        if side_channels is None:
            side_channels = []

        engine_channel = (
            EngineConfigurationChannel()
        )  # standard MLAgents unity configuration side channel
        engine_channel.set_configuration_parameters(
            width=display_width,
            height=display_height,
            quality_level=quality_level,
            time_scale=time_scale,
        )
        side_channels.append(engine_channel)

        self.config_channel = (
            UnityConfigChannel()
        )  # custom config side channel used to enable bugs/change agent behaviours
        side_channels.append(self.config_channel)

        stream = stdout if debug else _DevnullStream()
        log_channel = UnityLogChannel(stream)

        side_channels.append(log_channel)
        if seed is None:
            seed = np.random.randint(0, 1e8)  # set up environment with a random seed
        super().__init__(
            file_name=file_name,
            worker_id=worker_id,
            base_port=base_port,
            seed=seed,
            no_graphics=no_graphics,
            timeout_wait=timeout_wait,
            additional_args=additional_args,
            side_channels=side_channels,
            log_folder=log_folder,
            **kwargs,
        )

    def set_empty(
        self, behavior_name
    ) -> None:  # should be used in place of set_action if using a c# heuristic instead...
        self._assert_behavior_exists(behavior_name)
        if behavior_name not in self._env_state:
            return
        self._env_actions[behavior_name] = ActionTuple()

    def enable_bug(self, bug):
        msg = f"Bugs.{bug}.enabled:{True}"
        self.config_channel.write(str(msg))

    def disable_bug(self, bug):
        msg = f"Bugs.{bug}.enabled:{False}"
        self.config_channel.write(str(msg))

    def set_agent_behaviour(self, behaviour):
        msg = f"Heuristics.{behaviour}.heuristic:{True}"
        self.config_channel.write(str(msg))
        self.reset()  # reset the environment and ignore the result...
