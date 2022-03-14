rm -rf docs/Reference/
rm -rf tmp/xml

doxygen doxygen_config.txt           # install with sudo apt-get install doxygen
#doxybook -i tmp/xml -o docs/Reference/ -t mkdocs # install with pip install doxybook
moxygen -c -o docs/Reference/ tmp/xml/