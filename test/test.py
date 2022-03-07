#!/usr/bin/env python
# -*- coding: utf-8 -*-
""" 
   Created on 07-03-2022
"""
__author__ = "Benedict Wilkins"
__email__ = "benrjw@gmail.com"
__status__ = "Development"

from pprint import pprint
import worldofbugs as wob



info = wob.utils.BuildResolver.get_unity_environments()

pprint(info)
