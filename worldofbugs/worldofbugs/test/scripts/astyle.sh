#!/bin/sh
ASTYLE=$(which astyle)
if [ $? -ne 0 ]; then
	echo "astyle not installed, it can be installed with `sudo apt-get install astyle` or downloaded from http://astyle.sourceforge.net/"
	exit 1
fi
astyle com.worldofbugs.worldofbugs/*.cs --exclude=PackageCache --recursive --style=attach --suffix=none -s4 -f -p -xg -U -xe -j -xp -xC88 -xL -xe --verbose -Q -N