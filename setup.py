#!/usr/bin/env python
# -*- coding: utf-8 -*-
"""
    Created on 11-11-2020 16:33:31
"""
__author__ = "Benedict Wilkins"
__email__ = "benrjw@gmail.com"
__status__ = "Development"

from setuptools import find_packages, setup

# last successful build versions:
# gym-unity-0.28.0
# mlagents-envs-0.28.0
# unity 2020.3.25f1

GYM_NAMESPACE = "WOB"

setup(
    name="worldofbugs",
    version="0.0.1",
    description="",
    url="",
    author="Benedict Wilkins",
    author_email="benrjw@gmail.com",
    packages=find_packages(),
    install_requires=[
        "gym>=0.2.0",
        "gym-unity==0.28.0",
        "omegaconf",
        "loguru",
        "numpy",
    ],
    entry_points={"gym.envs": [f"{GYM_NAMESPACE} = worldofbugs:_register_entry_point"]},
    zip_safe=False,
)
