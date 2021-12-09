#!/bin/bash

DIRECTORY="/home/ubuntu/.aws"

# sync install files
rsync -rv ./ ubuntu@$1:$DIRECTORY

# update and add packages
ssh ubuntu@$1 "source $DIRECTORY/install_dpkg.sh $DIRECTORY && exit"

# install anaconda, restarting the connection creates another shell, which is important for anaconda setup...
ssh ubuntu@$1 "source $DIRECTORY/install_conda.sh && exit"

# install python packages
ssh ubuntu@$1 "source $DIRECTORY/install_python.sh  && exit"

# setup kaggle
rsync -rv ~/.kaggle/ ubuntu@$1:/home/ubuntu/.kaggle/
ssh ubuntu@$1 "source $DIRECTORY/kaggle_datasets.sh $DIRECTORY && exit"

# set up git repositories
ssh ubuntu@$1 "source $DIRECTORY/git_repos.sh $DIRECTORY && exit"


