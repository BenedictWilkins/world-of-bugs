#!/usr/bin/env python
# -*- coding: utf-8 -*-
"""
    Created on 11-10-2021 12:38:21

    [Description]
"""
__author__ = "Benedict Wilkins"
__email__ = "benrjw@gmail.com"
__status__ = "Development"


import numpy as np

import gym
from gym import spaces
from types import SimpleNamespace

from gym_unity.envs import UnityGymException
from prettytable import DEFAULT

from ._base import UnityEnvironment
from ._action import ActionHandler
from ._observation import ObservationHandler

from ..utils import WorldOfBugsException

DEFAULT_OBSERVATION_KEY = 'Observation' # used to unpack the agents observations, everything else goes into 'info'
DEFAULT_ACTION_KEY = 'Action'
DEFAULT_MASK_KEY = 'Mask'

__all__ = ("UnityGymEnvironment", "WOBEnvironment")

class UnityGymEnvironment(gym.Env):

    # TODO The multi-agent version is much more complicate to implement, mainly due to the asynchronous nature of decision requests...

    def __init__(self, env):
        super().__init__()
        assert isinstance(env, UnityEnvironment) # only the bugged unity environment is supported!
        self._env = env
 
        # inherit some methods from the wrapped environment
        setattr(self, self._env.enable_bug.__name__, self._env.enable_bug)
        setattr(self, self._env.disable_bug.__name__, self._env.disable_bug)
        setattr(self, self._env.set_agent_behaviour.__name__, self._env.set_agent_behaviour)

        # TAKEN FROM UnityToGymWrapper
        self._env.reset() # reset the environment so that the info below becomes avaliable
        if len(self._env.behavior_specs) > 1:
            raise UnityGymException(f"Only one behaviour spec is supported by this gym environment, found {list(self._env.behavior_specs.keys())}.")
        self.name = next(iter(self._env.behavior_specs.keys()))
        self.spec = self._env.behavior_specs[self.name]

        self._action_handler = ActionHandler(self.spec.action_spec)
        self._observation_handler = ObservationHandler(self.spec.observation_specs)
        self._data = SimpleNamespace(state=None, action=None, reward=None, done=False, info=None)
        
    def close(self):
        self._env.close()

    def reset(self):
        self._env.reset()
        self._data.done = False

        decision_steps, terminal_steps = self._env.get_steps(self.name)

        state, _, _, info = self._collect(None, decision_steps, terminal_steps)
        return state, info

    def step(self, action=None):
        if self._data.done:
            raise WorldOfBugsException("Attempted to call step when the environment is already done.")
        
        if action is not None:
            action_tuple = self._action_handler.get_action_tuple(action)
            self._env.set_actions(self.name, action_tuple)
        else:
            self._env.set_empty(self.name)

        self._env.step()
        decision_steps, terminal_steps = self._env.get_steps(self.name)

        self._data.done = len(terminal_steps) > 0
        self._data.state, self._data.action, self._data.reward, self._data.info = self._collect(action, decision_steps, terminal_steps)
        return self._data.state, self._data.reward, self._data.done, self._data.info 

    def _collect(self, action, decision_steps, terminal_steps):
        steps = decision_steps if len(decision_steps) > 0 else terminal_steps
        reward = steps.reward[0] if steps.reward.shape[0] > 0 else 0. # sometimes reward is not defined
        return steps.obs, action, reward, None # state, action, reward, info

    @property
    def observation_space(self):
        return self._observation_handler.observation_space

    @property
    def action_space(self):
        return self._action_handler.action_space


class WOBEnvironment(UnityGymEnvironment):

    def __init__(self, *args, observation_key=DEFAULT_OBSERVATION_KEY, **kwargs):
        super().__init__(*args, **kwargs)
        self._renderer = None

        if observation_key not in self._observation_handler.sensors:
            raise WorldOfBugsException(f"Invalid observation key {observation_key}, valid observations include {self._observation_handler.sensors}")
        self.observation_key = observation_key

        # there is lag at the begining of an episode, this is a hack to prevent issues with 
        # actions and observation lining up. This should be removed in favour of something less 
        # "hacky" in the future, probably implemented at the unity side. TODO
        self.reset()
        for i in range(50):
            self.step(0)
        self.reset()
        
    def _collect(self, action, decision_steps, terminal_steps):
        steps = decision_steps if len(decision_steps) > 0 else terminal_steps
        reward = steps.reward[0] if steps.reward.shape[0] > 0 else 0. # sometimes reward is not defined
        info = {k.split(":")[-1]:v.squeeze(0) for k,v in zip(self._observation_handler.sensors, steps.obs)}
        state = info[self.observation_key]
        del info[self.observation_key]
        #print(action, info[DEFAULT_ACTION_KEY])
        action = info.get(DEFAULT_ACTION_KEY) # if the policy used is a unity policy, this is important.
        return state, action, reward, info

    def render(self):
        if self._renderer is None:
            self._renderer = DebugRenderer(self, (400,200))
        self._renderer.step()

    @property
    def observation_space(self):
        # get the observation_space for the observation sensor
        return self._observation_handler[self.observation_key] 
    
class DebugRenderer:

    def __init__(self, env, display_size=(400,200)):
        self.env = env
        try:
            import pygame
        except:
            raise ImportError("To visualise a WOB environment the pygame package is required. It can be installed with `pip install pygame`, alternatively the you might consider running the environment with Unity editor.")
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
        obs = np.concatenate([self.env._data.state.transpose(1,0,2), self.env._data.info[DEFAULT_MASK_KEY].transpose(1,0,2)], axis=0)
        obs = (obs * 255).astype(np.uint8)
        surf = self.pygame.Surface(obs.shape[:2])
        self.pygame.surfarray.blit_array(surf, obs)
        surf = self.pygame.transform.scale(surf, self.display_size)
        self.display.blit(surf, (0,0))
        self.pygame.display.flip()
 
    def close(self):
        self.pygame.quit()

    def record(self, path):
        self.pygame.image.save(self.display, path)
        


