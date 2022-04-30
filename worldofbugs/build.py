#!/usr/bin/env python
# -*- coding: utf-8 -*-
"""
    Build all of the unity environments available in `UnityProject`. Environment builds are placed in `worldofbugs/builds`. 
    Building requires unity editor version `UNITY_VERSION` and the unity mlagents plugin `ML_AGENTS_VERSION`, 
    for details see [here](https://github.com/Unity-Technologies/ml-agents). Currently a build corresponds 
    to a particular scene and can only be built for linux.
    Alternatively, you can download builds from [here](https://www.kaggle.com/benedictwilkinsai/world-of-bugs). 
    Building requires the following: 
    ```
    UNITY_VERSION = "2020.3.25f1"
    ML_AGENTS_VERSION = "2.1.0-exp1 (Preview)"
    ```
"""

__author__ = "Benedict Wilkins"
__email__ = "benrjw@gmail.com"
__status__ = "Development"


""" DEPRECATED: TODO update to match v1.0

import argparse
import os
import subprocess

UNITY_PROJECT_DIR = "UnityProject"

parser = argparse.ArgumentParser(description='Process some integers.')
parser.add_argument('--path', '-p', type=str, required=True,
                    help="Path of the unity editor to use (please use version 2019.4.25f1 to avoid issues). The editor path can be found in the Unity Hub Settings. Example: '.../editors/2019.4.25f1/Editor/Unity'")

args = parser.parse_args()

project_path = os.path.split(__file__)[0]
project_path = os.path.join(project_path, "..", UNITY_PROJECT_DIR)
project_path = os.path.abspath(project_path)
command = f"{args.path} -projectpath {project_path} -executeMethod Build.Start" 
print(f"BUILDING UNITY SCENES USING COMMAND: {command}") # TODO use Logger
proc = subprocess.Popen(command, stdout=subprocess.PIPE, shell=True)
(out, err) = proc.communicate()
if out is not None:
    if len(out) > 0:
        print(out)
if err is not None:
    if len(err) > 0:
        print(err)
print("DONE.")
"""