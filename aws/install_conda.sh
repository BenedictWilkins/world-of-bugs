cd /tmp
curl -O https://repo.anaconda.com/archive/Anaconda3-2020.07-Linux-x86_64.sh
sha256sum Anaconda3-2020.07-Linux-x86_64.sh
bash Anaconda3-2020.07-Linux-x86_64.sh -b
export PATH="~/anaconda3/bin:$PATH"
cd ~

source $(conda info --base)/etc/profile.d/conda.sh
conda init bash
source ~/.bashrc

