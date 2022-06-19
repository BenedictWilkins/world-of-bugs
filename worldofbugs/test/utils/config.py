#!/usr/bin/env python
# -*- coding: utf-8 -*-
"""
   Created on 19-06-2022
"""
__author__ = "Benedict Wilkins"
__email__ = "benrjw@gmail.com"
__status__ = "Development"

import pathlib

__all__ = (
    "PYPI_PACKAGE_NAME",
    "PYPI_PACKAGE_PATH",
    "DEFAULT_CONFIG_PATH",
    "get_test_config",
)

PYPI_PACKAGE_NAME = "worldofbugs"
PYPI_PACKAGE_PATH = str(
    pathlib.Path(__file__).parent.parent.parent.expanduser().resolve()
)
DEFAULT_CONFIG_PATH = str(
    pathlib.Path(PYPI_PACKAGE_PATH, "test/config.yaml").expanduser().resolve()
)


def get_test_config(config_file=DEFAULT_CONFIG_PATH):
    from omegaconf import OmegaConf

    config_file = str(pathlib.Path(config_file).expanduser().resolve())
    config = OmegaConf.load(config_file)
    OmegaConf.update(
        config,
        "PATH",
        str(pathlib.Path(PYPI_PACKAGE_NAME).parent.resolve()),
        force_add=True,
    )
    OmegaConf.update(config, "PYPI_PACKAGE_NAME", PYPI_PACKAGE_NAME, force_add=True)
    OmegaConf.update(config, "PYPI_PACKAGE_PATH", PYPI_PACKAGE_PATH, force_add=True)
    OmegaConf.update(
        config,
        "UNITY_EDITOR_PATH",
        str(pathlib.Path(config.UNITY_EDITOR_PATH).expanduser().resolve()),
    )
    OmegaConf.resolve(config)
    return config


if __name__ == "__main__":
    from pprint import pprint

    from omegaconf import OmegaConf

    pprint(OmegaConf.to_container(get_test_config()))
