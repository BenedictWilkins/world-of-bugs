#!/usr/bin/env python
# -*- coding: utf-8 -*-
""" 
   Created on 08-03-2022
"""
__author__ = "Benedict Wilkins"
__email__ = "benrjw@gmail.com"
__status__ = "Development"


from ._observation import *
from ._action import *
from ._base import *
from ._environment import *

__all__ = ('make',)

def make(env_id, worker=0, display_width=84, display_height=84, quality_level=3, time_scale=1.0, log_folder=None, debug=True): 
   from ..utils import BuildResolver
   if env_id is not None:
      build = BuildResolver.get_build(env_id)
      ex_path = build.path
   else:
      ex_path = None # USE THE UNITY EDITOR
   env = UnityEnvironment(file_name=ex_path, 
                                    worker_id=worker, 
                                    log_folder=log_folder, 
                                    display_height=display_height, 
                                    display_width=display_width, 
                                    quality_level=quality_level, 
                                    time_scale=time_scale, 
                                    debug=debug)
   env = WOBEnvironment(env)
   return env

