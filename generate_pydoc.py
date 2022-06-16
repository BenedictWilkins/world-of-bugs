"""
    Generate markdown from Python doc strings with pydoc_markdown.
"""
from loguru import logger
from pydoc_markdown import PydocMarkdown
from pydoc_markdown.contrib.loaders.python import PythonLoader
from pydoc_markdown.contrib.renderers.markdown import MarkdownRenderer

PYDOC_PATH = "docs/Reference/Python"

session = (
    PydocMarkdown()
)  # Preconfigured with a PythonLoader, FilterProcessor, CrossRefProcess, SmartProcessor and MarkdownRenderer

assert isinstance(session.loaders[0], PythonLoader)
session.loaders[0].packages = ["worldofbugs"]

assert isinstance(session.renderer, MarkdownRenderer)
session.renderer.use_fixed_header_levels = True
session.renderer.header_level_by_type = {
    "Module": 2,
    "Class": 3,
    "Method": 4,
    "Function": 3,
    "Variable": 4,
}
session.renderer.render_page_title = True
session.renderer.descriptive_class_title = "Class "
session.renderer.descriptive_module_title = "Module "
session.renderer.render_toc = False

session.renderer.add_module_prefix = False
session.renderer.add_method_class_prefix = False
session.renderer.add_full_prefix = False
modules = session.load_modules()
modules = [m for m in sorted(modules, key=lambda m: m.name)]
modules = [m for m in modules if not "# MKDOCS IGNORE MODULE" in str(m.docstring)]
modules = [m for m in modules if "_" not in m.name]

session.process(modules)

with open(f"{PYDOC_PATH}/index.md", "w") as index:
    session.renderer.render_single_page(index, [modules[0]], page_title=modules[0].name)

for i, m in enumerate(modules[1:]):
    name = ".".join(m.name.split(".")[1:]).replace("_", "")
    logger.debug(f"Documenting: {name}")
    session.renderer.render_single_page(
        open(f"{PYDOC_PATH}/{i}_{name}.md", "w"), [m], page_title=name
    )
