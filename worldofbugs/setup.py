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
# unity 2020.3.29f1

GYM_NAMESPACE = "WOB"

setup(
    name="worldofbugs",
    version="0.0.1",
    description="World of Bugs (WOB) is a platform that supports research in Automated Bug Detection (ABD) in video games.",
    url="https://benedictwilkins.github.io/world-of-bugs/",
    author="Benedict Wilkins",
    author_email="benrjw@gmail.com",
    packages=find_packages(),
    install_requires=[
        "protobuf==3.20.*",
        "gym>=0.21.0",
        "gym_unity",
        "omegaconf",
        "loguru",
        "numpy",
        "xmltodict",
        "anybadge",
        "pylint",
        "coverage",
    ],
    exclude=["worldofbugs.test"],
    entry_points={
        "gym.envs": [f"{GYM_NAMESPACE} = worldofbugs._entry_point:register_entry_point"]
    },
    zip_safe=False,
)
