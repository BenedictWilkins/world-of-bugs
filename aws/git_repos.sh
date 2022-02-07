#/bin/bash
INSTALL_DIR=$1 
source /home/ubuntu/anaconda3/etc/profile.d/conda.sh
conda activate PhD

# Clones any git repositories in the repos.txt file, they must be public (https), or ssh properly set up.
cd ~
mkdir -p repos
cd repos
while read repo; do 
    git clone "$repo"
    name=$(echo "$repo" | rev | cut -d'/' -f 1 | rev | cut -d'.' -f 1)
    pip install -e ./$name
done < "$INSTALL_DIR/repos.txt"

