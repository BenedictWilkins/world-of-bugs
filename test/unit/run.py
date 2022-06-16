#!/usr/bin/env python
# -*- coding: utf-8 -*-
""" 
   Created on 16-06-2022
"""
__author__ = "Benedict Wilkins"
__email__ = "benrjw@gmail.com"
__status__ = "Development"

import argparse
import yaml
import pathlib
import subprocess
import os
import sys
from omegaconf import OmegaConf



PATH_BASE = str(pathlib.Path(__file__).parent.parent.parent) # root repo directory
PATH_BASE_TEST = str(pathlib.Path(PATH_BASE, "test"))
DEFAULT_CONFIG_FILE = str(pathlib.Path(f"{PATH_BASE_TEST}/default_config.yaml").resolve())
parser = argparse.ArgumentParser()
parser.add_argument("--config", type=str, default=DEFAULT_CONFIG_FILE)

args = parser.parse_args()
config = OmegaConf.load(args.config)
OmegaConf.update(config, "PATH", PATH_BASE)
OmegaConf.resolve(config)

unity_editor_cmd = config['UNITY_EDITOR_PATH'] + " -projectpath " + config['PROJECT_PATH']
proc = subprocess.Popen(unity_editor_cmd, stdout=subprocess.PIPE, shell=True, preexec_fn=os.setsid, close_fds=True)

while True:
  line = proc.stdout.readline()
  if not line:
    break
  print(line.rstrip())