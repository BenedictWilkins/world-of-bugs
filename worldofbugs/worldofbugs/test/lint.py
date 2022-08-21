#!/usr/bin/env python
# -*- coding: utf-8 -*-
"""
    Lint .py files.
"""
__author__ = "Benedict Wilkins"
__email__ = "benrjw@gmail.com"
__status__ = "Development"

if __name__ == "__main__":

    import pathlib
    import sys

    import anybadge
    from pylint.lint import Run

    from worldofbugs.test.utils import get_test_config

    TEST_CONFIG = get_test_config()

    # path = str(
    #    pathlib.Path(pathlib.Path(__file__).parent.parent, TEST_CONFIG.PIPY_PACKAGE_NAME).resolve()
    # )
    # test_path = str(pathlib.Path(pathlib.Path(__file__).parent).resolve())

    results = Run([TEST_CONFIG.PYPI_PACKAGE_PATH], do_exit=False)
    final_score = results.linter.stats.global_note

    thresholds = {2: "red", 4: "orange", 6: "yellow", 10: "green"}

    badge = anybadge.Badge("pylint", round(final_score, 2), thresholds=thresholds)
    badge.write_badge(f"{TEST_CONFIG.PATH}/docs/_badge/pylint.svg", overwrite=True)

    if final_score < TEST_CONFIG.PYLINT_THRESHOLD:
        print("------------------------------------------------------------------")
        print("Lint failed, please clean up your code!")
        print("------------------------------------------------------------------")
        sys.exit(-1)
