#!/usr/bin/env python
# -*- coding: utf-8 -*-
"""
    Created on 11-10-2021 12:38:21

    [Description]
"""
__author__ = "Benedict Wilkins"
__email__ = "benrjw@gmail.com"
__status__ = "Development"


from types import SimpleNamespace

import gym
import numpy as np
from gym_unity.envs import UnityGymException

from ..utils import WorldOfBugsException
from ._action import ActionHandler
from ._base import UnityEnvironment
from ._observation import ObservationHandler

DEFAULT_OBSERVATION_KEY = "Observation"  # used to unpack the agents observations, everything else goes into 'info'
DEFAULT_ACTION_KEY = "Action"
DEFAULT_MASK_KEY = "Mask"

__all__ = ("UnityGymEnvironment", "WOBEnvironment")

# TODO The multi-agent version is much more complicate to implement, mainly due to the asynchronous nature of decision requests...
class UnityGymEnvironment(gym.Env):
    """Wrapper for a UnityEnvironment."""

    def __init__(self, env):
        super().__init__()
        # only the bugged unity environment is supported!
        assert isinstance(env, UnityEnvironment)
        self._env = env

        # TAKEN FROM UnityToGymWrapper
        self._env.reset()  # reset the environment so that the info below becomes avaliable
        if len(self._env.behavior_specs) > 1:
            raise UnityGymException(
                f"Only one behaviour spec is supported by this gym environment, found {list(self._env.behavior_specs.keys())}."
            )
        self.name = next(iter(self._env.behavior_specs.keys()))
        self.spec = self._env.behavior_specs[self.name]

        self._action_handler = ActionHandler(self.spec.action_spec)
        self._observation_handler = ObservationHandler(self.spec.observation_specs)
        self._data = SimpleNamespace(
            state=None, action=None, reward=None, done=False, info=None
        )

    def enable_bug(self, bug):
        """Enable a bug by name in this WOB environment. The bug will be immediately enabled.
        Args:
            bug (str): bug name.
        """
        self._env.enable_bug(bug)

    def disable_bug(self, bug):
        """Disable a bug by name in this WOB environment. The bug will be immediately disabled.
        Args:
            bug (str): bug name.
        """
        self._env.disable_bug(bug)

    def set_agent_behaviour(self, behaviour):
        """Set the behaviour of the WOB agent. This will set the environment to heuristic mode. Any actions taken by a python agent (in `env.step(...)`) will be ignored. The action taken by the heuristic can be found in `info['Action']` from `obs, reward, done, info = env.step(action)`
        Args:
            behaviour (str): behaviour name.
        """
        self._env.set_agent_behaviour(behaviour)

    def close(self):
        """
        Shutdown this environment, terminating the WOB environment process or stopping the run in the unity editor.
        """
        self._env.close()

    def reset(self):
        self._env.reset()
        self._data.done = False
        decision_steps, terminal_steps = self._env.get_steps(self.name)
        state, _, _, info = self._collect(None, decision_steps, terminal_steps)
        return state, info

    def step(self, action=None):
        if self._data.done:
            raise WorldOfBugsException(
                "Attempted to call step when the environment is already done."
            )

        action_tuple = self._action_handler.get_action_tuple(action)
        self._env.set_actions(self.name, action_tuple)

        self._env.step()
        decision_steps, terminal_steps = self._env.get_steps(self.name)

        self._data.done = len(terminal_steps) > 0
        (
            self._data.state,
            self._data.action,
            self._data.reward,
            self._data.info,
        ) = self._collect(action, decision_steps, terminal_steps)
        return self._data.state, self._data.reward, self._data.done, self._data.info

    def _collect(self, action, decision_steps, terminal_steps):
        steps = decision_steps if len(decision_steps) > 0 else terminal_steps
        reward = (
            steps.reward[0] if steps.reward.shape[0] > 0 else 0.0
        )  # sometimes reward is not defined
        return steps.obs, action, reward, None  # state, action, reward, info

    @property
    def observation_space(self):
        """Get environment observation space.
        Returns:
            gym.spaces.Space: observation space
        """
        return self._observation_handler.observation_space

    @property
    def action_space(self):
        """Get environment action space.
        Returns:
            gym.spaces.Space: action space
        """
        return self._action_handler.action_space

    def render(self, *args, **kwargs):
        raise NotImplementedError()


class WOBEnvironment(UnityGymEnvironment):
    """Single agent WOB environment that follows the OpenAI gym environment API."""

    def __init__(self, *args, observation_key=DEFAULT_OBSERVATION_KEY, **kwargs):
        super().__init__(*args, **kwargs)
        self._renderer = None

        if observation_key not in self._observation_handler.sensors:
            raise WorldOfBugsException(
                f"Invalid observation key {observation_key}, valid observations include {self._observation_handler.sensors}"
            )
        self.observation_key = observation_key

        # TODO there is lag at the begining of an episode, this is a hack to work around the issue.
        # This should be removed in favour of something less "hacky" in the future, probably implemented at the unity side.
        self.reset()
        for _ in range(100):
            self.step(0)
        self.reset()

    def _collect(self, action, decision_steps, terminal_steps):
        steps = decision_steps if len(decision_steps) > 0 else terminal_steps
        reward = (
            steps.reward[0] if steps.reward.shape[0] > 0 else 0.0
        )  # sometimes reward is not defined
        info = {
            k.split(":")[-1]: v.squeeze(0)
            for k, v in zip(self._observation_handler.sensors, steps.obs)
        }
        state = info[self.observation_key]
        del info[self.observation_key]
        # print(action, info[DEFAULT_ACTION_KEY])
        action = info.get(
            DEFAULT_ACTION_KEY
        )  # if the policy used is a unity policy, this is important.
        return state, action, reward, info

    def render(self, *args, **kwargs):
        if self._renderer is None:
            self._renderer = DebugRenderer(self, (400, 200))
        self._renderer.step(*args, **kwargs)

    @property
    def observation_space(self):
        return self._observation_handler[self.observation_key]


class DebugRenderer:
    def __init__(self, env, display_size=(400, 200)):
        self.env = env
        try:
            import pygame
        except:
            raise ImportError(
                "To visualise a WOB environment the pygame package is required. It can be installed with `pip install pygame`, alternatively the you might consider running the environment with Unity editor."
            )
        self.pygame = pygame
        self.pygame.init()
        self.display_size = display_size
        self.display = self.pygame.display.set_mode(self.display_size)

    def step(self):
        for event in self.pygame.event.get():
            if event.type == self.pygame.QUIT:
                self.close()
                return
        # otherwise, render the current observation
        obs = np.concatenate(
            [
                self.env._data.state.transpose(1, 0, 2),
                self.env._data.info[DEFAULT_MASK_KEY].transpose(1, 0, 2),
            ],
            axis=0,
        )
        obs = (obs * 255).astype(np.uint8)
        surf = self.pygame.Surface(obs.shape[:2])
        self.pygame.surfarray.blit_array(surf, obs)
        surf = self.pygame.transform.scale(surf, self.display_size)
        self.display.blit(surf, (0, 0))
        self.pygame.display.flip()

    def close(self):
        self.pygame.quit()

    def record(self, path):
        self.pygame.image.save(self.display, path)
