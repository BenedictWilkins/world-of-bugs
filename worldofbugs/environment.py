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

from mlagents_envs.environment import UnityEnvironment
from typing import Dict, List, Optional, Tuple, Mapping as MappingType
from mlagents_envs.side_channel.side_channel import SideChannel
from mlagents_envs.side_channel.engine_configuration_channel import EngineConfigurationChannel
from mlagents_envs.environment import UnityEnvironment
from gym_unity.envs import UnityToGymWrapper, ActionFlattener, ActionTuple, UnityGymException

from .sidechannel import UnityLogChannel, UnityConfigChannel

class ActionHandler:

    def get_action_tuple(self, actions):
        raise NotImplementedError()

class ContinuousActionHandler(ActionHandler):

    def __init__(self, spec):
        self.action_spec = spec.action_spec
        self.action_size = self.action_spec.continuous_size
        high = np.array([1] * self.action_spec.continuous_size)
        self.action_space = spaces.Box(-high, high, dtype=np.float32)

    def get_action_tuple(self, actions):
        return ActionTuple(continuous=actions) # simple!

class DiscreteActionHandler(ActionHandler): # TODO support both discrete and continuous actions...

    def __init__(self, spec):
        self.action_spec = spec.action_spec
        # always flatten action space? 
        self.action_size = self.action_spec.discrete_size
        branches = self.action_spec.discrete_branches
        if self.action_size == 1:
            self._flattener = None
            self.action_space = spaces.Discrete(branches[0])
        else:            
            self._flattener = ActionFlattener(branches)
            self.action_space = self._flattener.action_space

    def get_action_tuple(self, actions):
        if self._flattener is not None:
            # bit of a shame this needs to happen... probably just dont use branches in unity... why are they even there!
            actions = np.array([self._flattener.lookup_action(action) for action in actions])
        actions = actions.reshape(actions.shape[0], self.action_size)
        
        return ActionTuple(discrete=actions)

class SingleAgentUnityGymEnvironment(gym.Env):

    # TODO The multi-agent version is much more complicate to implement, mainly due to the asynchronous nature of decision requests...

    def __init__(self, env):
        super().__init__()
        assert isinstance(env, BuggedUnityEnvironment) # only the bugged unity environment is supported!
        self._env = env
        #self._env.set_player_behaviour('ML')
        
        # inherit some methods from the wrapped environment
        setattr(self, self._env.enable_bug.__name__, self._env.enable_bug)
        setattr(self, self._env.disable_bug.__name__, self._env.disable_bug)
        setattr(self, self._env.set_player_behaviour.__name__, self._env.set_player_behaviour)

        # TAKEN FROM UnityToGymWrapper
        self._env.reset() # reset the environment so that the info below becomes avaliable
        if len(self._env.behavior_specs) > 1:
            raise UnityGymException(f"Only one behaviour spec is supported by this gym environment, found {list(self._env.behavior_specs.keys())}.")
        self.name = list(self._env.behavior_specs.keys())[0]
        self.spec = self._env.behavior_specs[self.name]

        self.__init_action_space()
        self.__init_observation_space()

        self._data = SimpleNamespace(state=None, action=None, reward=None, done=False, info=None)
        
    def close(self):
        self._env.close()

    def __init_action_space(self):
        if self.spec.action_spec.is_continuous():
            self._action_handler = ContinuousActionHandler(self.spec)
        elif self.spec.action_spec.is_discrete():
            self._action_handler = DiscreteActionHandler(self.spec)
        else:
            # same as UnityToGymWrapper, its annoying to implement...
            raise UnityGymException("This gym wrapper does not provide explicit support for both discrete and continuous actions.")

    def __init_observation_space(self):
        list_spaces = []
        # visual observations...
        for shape in [spec.shape for spec in self.spec.observation_specs if len(spec.shape) == 3]:
            list_spaces.append(spaces.Box(0, 1, dtype=np.float32, shape=shape))
        # vector observations...
        for size in [spec.shape[0] for spec in self.spec.observation_specs if len(spec.shape) == 1]:
            high = np.array([np.inf] * size)
            list_spaces.append(spaces.Box(-high, high, dtype=np.float32))
        self.observation_space = spaces.Tuple(list_spaces)

    def reset(self):
        self._env.reset()
        self._data.done = False
        decision_steps, terminal_steps = self._env.get_steps(self.name)
        #assert len(terminal_steps) == 0 # ???
        state, _, _, info = self._collect(None, decision_steps, terminal_steps)
        self._data.state, self._data.info = info
        return state, info

    def step(self, action):
        if self._data.done:
            raise UnityGymException("Attempted to call step when the environment is already done.")

        action_tuple = self._action_handler.get_action_tuple(np.array([action]))
        self._env.set_actions(self.name, action_tuple)
        self._env.step()
        decision_steps, terminal_steps = self._env.get_steps(self.name)
        self._data.done = len(terminal_steps) > 0
        state, action, reward, info = self._collect(action, decision_steps, terminal_steps)
        self._data.state, self._data.action, self._data.reward, self._data.info = state, action, reward, info
        return state, reward, self._data.done, info

    def _collect(self, action, decision_steps, terminal_steps):
        steps = decision_steps if len(decision_steps) > 0 else terminal_steps
        if steps.reward.shape[0] > 0:
            reward = steps.reward[0] # sometimes the reward is not defined
        else:
            reward = 0.
        return steps.obs, action, reward, None

    @property
    def action_space(self):
        return self._action_handler.action_space

class BuggedUnityGymEnvironment(SingleAgentUnityGymEnvironment):

    def __init__(self, *args, **kwargs):
        super().__init__(*args, **kwargs)
        self.sensors = [spec.name for spec in self.spec.observation_specs]
        self._renderer = None
        
    def _collect(self, action, decision_steps, terminal_steps):
        steps = decision_steps if len(decision_steps) > 0 else terminal_steps
        if steps.reward.shape[0] > 0: # sometimes the reward is not defined
            reward = steps.reward[0] 
        else:
            reward = 0.
        info = {k:v[0] for k,v in zip(self.sensors, steps.obs)}
        state = info['observation'] 
        del info['observation']
        action = info.get('info', [action])[0] # if the policy used is a unity policy, this is important. Otherwise, there is not difference here.
        return state, action, reward, info

    def render(self):
        if self._renderer is None:
            self._renderer = DebugRenderer(self, (400,200))
        self._renderer.step()

class DebugRenderer:

    def __init__(self, env, display_size=(400,200)):
        self.env = env
        try:
            import pygame
        except:
            raise ImportError("To visualise a WOB environment the pygame is required.")
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
        obs = np.concatenate([self.env._data.state.transpose(1,0,2), self.env._data.info['bugmask'].transpose(1,0,2)], axis=0)
        obs = (obs * 255).astype(np.uint8)
        surf = self.pygame.Surface(obs.shape[:2])
        self.pygame.surfarray.blit_array(surf, obs)
        surf = self.pygame.transform.scale(surf, self.display_size)
        self.display.blit(surf, (0,0))
        self.pygame.display.flip()
        
    def close(self):
        self.pygame.quit()





class BuggedUnityEnvironment(UnityEnvironment):

    def __init__(self, 
            file_name = None,
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
            **kwargs
            ):
        if side_channels is None:
            side_channels = []
            
        engine_channel = EngineConfigurationChannel()
        engine_channel.set_configuration_parameters(width=display_width,height=display_height, quality_level=quality_level,time_scale=time_scale)
        side_channels.append(engine_channel)

        self.config_channel = UnityConfigChannel()
        side_channels.append(self.config_channel)

        if debug:
            log_channel = UnityLogChannel()
            side_channels.append(log_channel)
        if seed is None:
            seed = np.random.randint(0, 1e8) # set up environment with a random seed
        super().__init__(file_name = file_name, 
                         worker_id = worker_id,
                         base_port = base_port,
                         seed = seed,
                         no_graphics = no_graphics,
                         timeout_wait = timeout_wait, 
                         additional_args = additional_args,
                         side_channels = side_channels,
                         log_folder = log_folder,
                         **kwargs)

    def enable_bug(self, bug):
        msg = f"{bug}:{True}"
        self.config_channel.write(str(msg))

    def disable_bug(self, bug):
        msg = f"{bug}:{False}"
        self.config_channel.write(str(msg))

    def set_player_behaviour(self, behaviour):
        msg = f"{behaviour}:{True}"
        self.config_channel.write(str(msg))
        self.reset() # reset the environment and ignore the result...