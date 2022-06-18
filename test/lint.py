#!/usr/bin/env python
# -*- coding: utf-8 -*-
"""
"""
__author__ = "Benedict Wilkins"
__email__ = "benrjw@gmail.com"
__status__ = "Development"

if __name__ == "__main__":

    import pathlib
    import sys

    import anybadge
    from pylint.lint import Run

    LINT_THRESHOLD = 8
    PACKAGE_NAME = "worldofbugs"

    path = str(
        pathlib.Path(pathlib.Path(__file__).parent.parent, PACKAGE_NAME).resolve()
    )
    test_path = str(pathlib.Path(pathlib.Path(__file__).parent).resolve())

    results = Run([path, test_path], do_exit=False)
    final_score = results.linter.stats.global_note

    thresholds = {2: "red", 4: "orange", 6: "yellow", 10: "green"}

    badge = anybadge.Badge("pylint", round(final_score, 2), thresholds=thresholds)
    badge.write_badge(f"{test_path}/badge/pylint.svg", overwrite=True)

    if final_score < LINT_THRESHOLD:
        print("------------------------------------------------------------------")
        print("Lint failed, please clean up your code!")
        print("------------------------------------------------------------------")
        sys.exit(-1)
