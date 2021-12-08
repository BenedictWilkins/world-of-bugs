# bin/bash

INSTALL_DIR=$1 
conda activate PhD
export PATH=$PATH:~/.local/bin
mkdir -p "~/datasets/"
cd "~/datasets/"
while read d; do 
    kaggle datasets download $d
    echo "Done."
done < "$INSTALL_DIR/dataset.txt"