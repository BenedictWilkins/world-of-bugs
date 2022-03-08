#!/usr/bin/env python
# -*- coding: utf-8 -*-
""" 
   Created on 08-03-2022
"""
__author__ = "Benedict Wilkins"
__email__ = "benrjw@gmail.com"
__status__ = "Development"

import numpy as np
import gym

from gym_unity.envs import UnityGymException, ActionTuple, ActionFlattener

__all__ = ("ActionHandler", "DiscreteActionHandler", "ContinuousActionHandler")

def ActionHandler(action_spec):
    if action_spec.is_continuous():
        return ContinuousActionHandler(action_spec)
    elif action_spec.is_discrete():
        return DiscreteActionHandler(action_spec)
    else: # same as UnityToGymWrapper, its annoying to implement...
        raise UnityGymException("This gym wrapper does not provide explicit support for both discrete and continuous actions.")
    
class _ActionHandler: # Attempt to streamline actions sent/received from unity. Why is branching even a thing!

    def get_action_tuple(self, actions):
        raise NotImplementedError()

# TODO support both discrete and continuous actions...

class ContinuousActionHandler(_ActionHandler):

    def __init__(self, action_spec):
        self.action_spec = action_spec
        self.action_size = self.action_spec.continuous_size
        high = np.array([1] * self.action_spec.continuous_size)
        self.action_space = gym.spaces.Box(-high, high, dtype=np.float32)

    def get_action_tuple(self, actions):
        return ActionTuple(continuous=actions) # simple!

class DiscreteActionHandler(_ActionHandler): 

    def __init__(self, action_spec):
        self.action_spec = action_spec
        # always flatten action space? 
        self.action_size = self.action_spec.discrete_size
        branches = self.action_spec.discrete_branches
        if self.action_size == 1:
            self._flattener = None
            self.action_space = gym.spaces.Discrete(branches[0])
        else:            
            self._flattener = ActionFlattener(branches)
            self.action_space = self._flattener.action_space

    def get_action_tuple(self, actions):
        if self._flattener is not None:
            # bit of a shame this needs to happen... probably just dont use branches in unity... why are they even there!
            actions = np.array([self._flattener.lookup_action(action) for action in actions])
        actions = actions.reshape(actions.shape[0], self.action_size)
        return ActionTuple(discrete=actions)
