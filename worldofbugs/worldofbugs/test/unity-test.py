#!/usr/bin/env python
# -*- coding: utf-8 -*-
"""
    Run all unity tests, delegating to Unity's test framework.
"""
__author__ = "Benedict Wilkins"
__email__ = "benrjw@gmail.com"
__status__ = "Development"

import pathlib
import subprocess
import sys

import xmltodict

from worldofbugs.test.utils import get_test_config

TEST_CONFIG = get_test_config()
TEST_RESULT_FILE = str(pathlib.Path(TEST_CONFIG.PATH, "logs/unity-test.xml"))
TEST_PLATFORM = "playmode"


def test():
    # TODO check that the editor is not open, and that this actually succeeds!
    cmd = f"{TEST_CONFIG.UNITY_EDITOR_PATH} -runTests -batchmode -projectPath {TEST_CONFIG.UNITY_PROJECT_PATH} -testResults {TEST_RESULT_FILE} -testPlatform {TEST_PLATFORM}"
    proc = subprocess.Popen(
        cmd,
        stdout=subprocess.PIPE,
        shell=True,
        # preexec_fn=os.setsid,
        close_fds=True,
    )
    proc.wait()


def format_result(suite, result, indent=0, verbose=2):
    if verbose == 0 or suite is None:
        return
    if isinstance(suite, list):
        for x in suite:
            format_result(x, result, indent=indent, verbose=verbose)
        return
    header = f"{' ' * indent}{suite.get('@fullname', 'Global').split('/')[-1]}"
    print(
        f"{header:<88} | {suite['@result']} | {suite['@passed']}/{suite['@total']}",
        file=result,
    )
    if verbose > 1:
        cases = suite.get("test-case", [])
        if isinstance(cases, dict):
            cases = [cases]
        for case in cases:
            from pprint import pprint

            header = f"{' ' * indent}- {case['@name']}"
            print(f"{header:<88} | {case['@result']} | ", file=result)
            if case["@result"] == "Failed":
                message = case["failure"]["message"]
                if message is not None:
                    print(f"{' ' * (indent + 4)}{message}")
                print(f"{' ' * (indent + 4)}{case['failure']['stack-trace']}")
    format_result(
        suite.get("test-suite", None), result, indent=indent + 2, verbose=verbose
    )


test()

with open(TEST_RESULT_FILE, "r") as f:
    test_result = xmltodict.parse(f.read())

with open(TEST_RESULT_FILE.replace(".xml", ".log"), "w") as f:
    suite = test_result["test-run"]
    print("-" * 50, "UNITY TEST", "-" * 50)
    format_result(suite, sys.stdout)

    if suite["@passed"] != suite["@total"]:
        exit(-1)
