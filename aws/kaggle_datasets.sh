# bin/bash
INSTALL_DIR=$1 

source /home/ubuntu/anaconda3/etc/profile.d/conda.sh
conda activate PhD
export PATH=$PATH:~/.local/bin

cd ~
mkdir -p "datasets"
cd "datasets"
while read d; do 
    kaggle datasets download $d
    unzip $d.zip
done < "$INSTALL_DIR/dataset.txt"

# set up wandb
cd ~
WANDBKEY=$(<.wandb/wandb.txt)
wandb login $WANDBKEY
