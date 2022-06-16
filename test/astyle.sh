ASTYLE=$(which astyle)
if [ $? -ne 0 ]; then
	echo "astyle not installed, it can be installed with sudo apt-get install astyle or downloaded from http://astyle.sourceforge.net/"
	exit 1
fi
astyle com.worldofbugs.worldofbugs/*.cs --dry-run --exclude=PackageCache --recursive --style=attach -s4 -f -p -xg -U -xe -j -xp -xC88 -xL -xe --verbose -N