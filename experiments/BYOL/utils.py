import numpy as np
import torch

from torch.utils.data import TensorDataset, ConcatDataset, DataLoader
from tqdm.auto import tqdm

import krate

DATASET_NAME = "benedictwilkinsai/world-of-bugs/"
DATASET_ACR = "WOB"

def dataset(config):
    if not krate.exists("WOB"):
        krate.kaggle.download(DATASET_NAME)
        krate.registry.register(DATASET_ACR, DATASET_NAME)
     
    kdataset = krate.dataset(DATASET_ACR)
    FILE = kdataset.files[1] # small file... (50k)
    episodes = [torch.from_numpy(ep['observation'][...]).to(config.device, non_blocking=True)
                    for ep in tqdm(next(kdataset[FILE].load())[1].values())]
    dataset = ConcatDataset([TensorDataset(ep) for ep in episodes])
    loader = DataLoader(dataset, batch_size=config.batch_size, shuffle=True, drop_last=True)
    return dataset, loader


def test_dataset(config, n=1, shuffle=False):
    if not krate.exists("WOB"):
        krate.kaggle.download(DATASET_NAME)
        krate.registry.register(DATASET_ACR, DATASET_NAME)
     
    kdataset = krate.dataset(DATASET_ACR)
    kdataset = kdataset[kdataset.dirs[0]]
    def get_observations(h5file, n=1):
        for ep in list(h5file.values())[:n]:
            yield torch.from_numpy(ep['observation'][...]), torch.from_numpy(ep['bugmask'][...])
            
    episodes = [ep for k,h5file in tqdm(kdataset.load()) for ep in get_observations(h5file, n=n)]
    
    dataset = ConcatDataset([TensorDataset(*ep) for ep in episodes])
    loader = DataLoader(dataset, batch_size=config.batch_size, shuffle=shuffle, drop_last=False)
    return dataset, loader
    
    