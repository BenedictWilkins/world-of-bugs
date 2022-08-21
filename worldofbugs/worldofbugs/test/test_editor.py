#!/usr/bin/env python
# -*- coding: utf-8 -*-
"""
   Created on 16-06-2022
"""
__author__ = "Benedict Wilkins"
__email__ = "benrjw@gmail.com"
__status__ = "Development"

import argparse
import pathlib
import subprocess

from worldofbugs.test.utils import get_test_config

TEST_CONFIG = get_test_config()
import argparse

parser = argparse.ArgumentParser()
parser.add_argument("--scene", type=str, default="World-v1")
parser.add_argument("--play", action="store_true", default=False)
args = parser.parse_args()

cmd = f"{TEST_CONFIG.UNITY_EDITOR_PATH} -projectPath {TEST_CONFIG.UNITY_PROJECT_PATH} -executeMethod WorldOfBugs.Editor.SceneLoader.Main --scene {args.scene} --run {args.play}"

proc = subprocess.Popen(
    cmd,
    stdout=subprocess.PIPE,
    shell=True,
    # preexec_fn=os.setsid,
    close_fds=True,
)

proc.wait()
