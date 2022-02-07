
# weird bugs here...? need to restart the terminal for it to work properly? ehh
source /home/ubuntu/anaconda3/etc/profile.d/conda.sh
conda init bash
source ~/.bashrc

conda create -n PhD -y python=3.7
conda activate PhD

conda install jupyterlab -y
conda install jupyter -y
conda install ipykernel -y

pip install jupyter notebook
pip install environment_kernels

pip install ipywidgets
jupyter nbextension enable --py widgetsnbextension
python -m ipykernel install --user --name=PhD

conda install pytorch cudatoolkit=10.2 -c pytorch -y
pip install torchsummary
pip install torchvision

pip install kaggle
pip install wandb

