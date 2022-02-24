#!/usr/bin/env python
# -*- coding: utf-8 -*-
"""
    Build all of the unity environments. Environment builds are placed in worldofbugs/builds. 
    Building requires unity editor version 2019.4.25f1 and the unity mlagents plugin, 
    for details see https://github.com/Unity-Technologies/ml-agents

    On linux, the unity image can be installed using unityhub with the following command:
    './UnityHub.AppImage unityhub://2019.4.25f1/01a0494af254'

    Created on 06-10-2021 14:45:22
"""
__author__ = "Benedict Wilkins"
__email__ = "benrjw@gmail.com"
__status__ = "Development"

# /home/ben/Documents/repos/unity/editors/2019.4.25f1/Editor/Unity -batchmode -projectPath "/home/ben/Documents/repos/WorldOfBugs/UnityProject" -executeMethod Build.Start

# TODO add options for which scene to build?
# TODO add plaform specific options e.g. build target, see UnityProject/Editor/Build.cs

import argparse
import os
import subprocess

UNITY_PROJECT_DIR = "UnityProject"

parser = argparse.ArgumentParser(description='Process some integers.')
parser.add_argument('--path', '-p', type=str,
                    help="Path of the unity editor to use (please use version 2019.4.25f1 to avoid issues). The editor path can be found in the Unity Hub Settings. Example: '.../editors/2019.4.25f1/Editor/Unity'")

args = parser.parse_args()

project_path = os.path.split(__file__)[0]
project_path = os.path.join(project_path, "..", UNITY_PROJECT_DIR)
project_path = os.path.abspath(project_path)

# run the build command
# TODO batch mode? 
#command = f"{args.path} -batchmode -quit -projectpath {project_path} -executeMethod Build.Start" 
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