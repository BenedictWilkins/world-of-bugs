#!/bin/bash

DIRECTORY="/home/ubuntu/.aws"

# sync install files
rsync -rv ./ ubuntu@$1:$DIRECTORY

# run the install script on the server
ssh ubuntu@$1 "source $DIRECTORY/install.sh $DIRECTORY"

# setup kaggle
rsync -rv ~/.kaggle/ ubuntu@$1:/home/ubuntu/.kaggle/
ssh ubuntu@$1 "source $DIRECTORY/kaggle_datasets.sh $DIRECTORY"