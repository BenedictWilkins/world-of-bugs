"""Make some badges... :) """
import anybadge

BADGE_PATH = "docs/_badge"

badge = anybadge.Badge("codestyle", "astyle", default_color="navy")
badge.write_badge(f"{BADGE_PATH}/astyle.svg", overwrite=True)
