#/bin/bash

# Clones any git repositories in the repos.txt file, they must be public (https), or ssh properly set up.
INSTALL_DIR=$1 
conda activate PhD
mkdir -p "~/repos/"
cd "~/repos/"
while read d; do 
    git clone "$repo"
    name=$(echo "$repo" | rev | cut -d'/' -f 1 | rev | cut -d'.' -f 1)
    pip install -e ./$name
done < "$INSTALL_DIR/repos.txt"

