#!/usr/bin/env python
# -*- coding: utf-8 -*-
"""
    Build all of the unity environments. Environment builds are placed in worldofbugs/builds. 
    Building requires unity editor version 2019.4.25f1 and the unity mlagents plugin, 
    for details see https://github.com/Unity-Technologies/ml-agents

    On linux, the correct unity image can be installed using unityhub, or with the following command:
    './UnityHub.AppImage unityhub://2019.4.25f1/01a0494af254'

    Alternatively, you can download builds from https://www.kaggle.com/benedictwilkinsai/world-of-bugs
    All builds should be placed in the worldofbugs/builds directory so that they can be found by `worldofbugs.utils.make`
"""
__author__ = "Benedict Wilkins"
__email__ = "benrjw@gmail.com"
__status__ = "Development"

# /home/ben/Documents/unity/2019.4.25f1/Editor/Unity
# TODO add options for which scene to build?
# TODO add plaform specific options e.g. build target, see UnityProject/Editor/Build.cs

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
print(f"BUILDING UNITY SCENES USING COMMAND: {command}")
proc = subprocess.Popen(command, stdout=subprocess.PIPE, shell=True)
(out, err) = proc.communicate()
if out is not None:
    if len(out) > 0:
        print(out)
if err is not None:
    if len(err) > 0:
        print(err)
print("DONE.")