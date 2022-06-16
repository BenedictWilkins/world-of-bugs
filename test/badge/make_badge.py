"""Make some badges... :) """
import anybadge

badge = anybadge.Badge("codestyle", "astyle", default_color="navy")
badge.write_badge(f"test/badge/astyle.svg", overwrite=True)
