#!/usr/bin/env python
# -*- coding: utf-8 -*-
"""
    Build all of the unity environments available in `UnityProject`. Environment builds are placed in `worldofbugs/builds`. Building requires unity editor version `UNITY_VERSION` and the unity mlagents plugin `ML_AGENTS_VERSION`, for details see [here](https://github.com/Unity-Technologies/ml-agents). Currently a build corresponds to a particular scene and can only be built for linux.Alternatively, you can download builds from [here](https://www.kaggle.com/benedictwilkinsai/world-of-bugs). Building requires the following:
    ```
    UNITY_VERSION = "2020.3.25f1"
    ML_AGENTS_VERSION = "2.1.0-exp1 (Preview)"
    ```
"""

__author__ = "Benedict Wilkins"
__email__ = "benrjw@gmail.com"
__status__ = "Development"

import argparse
import glob
import pathlib
import subprocess

from loguru import logger

DEFAULT_EDITOR_VERSION = "2020.3.29f1"
DEFAULT_EDITOR_PATH = f"~/Unity/Hub/Editor/{DEFAULT_EDITOR_VERSION}/Editor/Unity"
DEFAULT_PROJECT_PATH = "com.worldofbugs.worldofbugs/Samples~/3D/"


def resolve_scenes(path):
    scenes = []
    for scene in glob.glob(path, recursive="**" in path):
        logger.debug(f"FOUND SCENE: {scene}")
        scenes.append(scenes)
    return scenes


parser = argparse.ArgumentParser(
    description="Build standalone WOB environments. Requires Unity3D to be installed."
)
parser.add_argument(
    "--editor",
    "-e",
    type=str,
    default=DEFAULT_EDITOR_PATH,
    help=f"Path of the unity editor to use (please use version {DEFAULT_EDITOR_VERSION} to avoid issues). The editor path can be found in the Unity Hub Settings. Example: '.../{DEFAULT_EDITOR_VERSION}/Editor/Unity'",
)

parser.add_argument(
    "--project",
    "-p",
    type=str,
    default=DEFAULT_PROJECT_PATH,
    help=f"Path to the unity project that should be built. Each scene in the project will be built seperately as a new environment. This is a limitation which will be fixed in later versions.",
)

args = parser.parse_args()
args.editor = str(pathlib.Path(args.editor).expanduser().resolve())
args.project = str(pathlib.Path(args.project).expanduser().resolve())

command = f"{args.editor} -projectpath {args.project} -executeMethod Build.Start"

logger.debug(f"BUILDING USING COMMAND: {command}")  # TODO use Logger

proc = subprocess.Popen(command, stdout=subprocess.PIPE, shell=True)
(out, err) = proc.communicate()
if out is not None:
    if len(out) > 0:
        print(out)
if err is not None:
    if len(err) > 0:
        print(err)
