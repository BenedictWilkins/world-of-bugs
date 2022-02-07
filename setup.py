#!/usr/bin/env python
# -*- coding: utf-8 -*-
"""
    Created on 11-11-2020 16:33:31
"""
__author__ = "Benedict Wilkins"
__email__ = "benrjw@gmail.com"
__status__ = "Development"

from setuptools import setup, find_packages

setup(name='worldofbugs',
      version='0.0.1',
      description='',
      url='',
      author='Benedict Wilkins',
      author_email='benrjw@gmail.com',
      packages=find_packages(),
      install_requires=['numpy', 'gym', 'h5py', 'torch', 'torchvision', 'ray', 'gym_unity'],
      zip_safe=False)


