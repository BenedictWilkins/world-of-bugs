source $1/waitfordpkg.sh

sudo apt update
sudo apt-get update
sudo apt-get autoremove

sudo apt-get install git
sudo apt-get install ffmpeg -y

# install anaconda
source "$1/install_conda.sh"

# install python packages
source "$1/install_python.sh"

# install git repositories
# source "$1/install_conda.sh $1"

