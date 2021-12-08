#!/usr/bin/env python
# -*- coding: utf-8 -*-
"""
    Created on 29-11-2021 13:44:44

    [Description]
"""
__author__ = "Benedict Wilkins"
__email__ = "benrjw@gmail.com"
__status__ = "Development"

import os
import h5py
from tqdm.auto import tqdm
import numpy as np
import torch
import torch.nn as nn
import torch.nn.functional as F

# download from kaggle... ?
WORLDOFBUGS_PATH = os.path.abspath("..")
DEFAULT_TRAIN_FILE_SMALL = "worldofbugs/builds/World-v1/dataset/NORMAL-50k.hdf5"
DEFAULT_TRAIN_FILE_LARGE = "worldofbugs/builds/World-v1/dataset/NORMAL-300k.hdf5"
DEFAULT_TEST_FILE = "worldofbugs/builds/World-v1/dataset/dataset-1.hdf5"

def load_train(file=os.path.join(WORLDOFBUGS_PATH, DEFAULT_TRAIN_FILE_SMALL), keys=["observation", "action"]):
    return [ep for ep in load(file, keys)]

def load_test(file=os.path.join(WORLDOFBUGS_PATH, DEFAULT_TEST_FILE), keys=["observation", "bugmask"]):
    return [ep for ep in load(file, keys)]

def load(file, keys, lazy=False):
    with h5py.File(file, 'r') as f:
        for g in tqdm(f):
            if not lazy:
                yield tuple([f[g][k][...] for k in keys])
            else:
                yield tuple([f[g][k] for k in keys])
                


class AE(nn.Module):
    
    def __init__(self, input_shape, latent_shape=1024):
        super().__init__()
        assert input_shape == (3,84,84)
        self.input_shape = as_shape(input_shape)
        self.latent_shape = as_shape(latent_shape)
        self.output_Shape = self.input_shape
        
        self.encoder = Encoder(self.input_shape, self.latent_shape)
        self.decoder = self.encoder.inverse()
        
    def forward(self, x):
        z = self.encoder(x)
        y = self.decoder(z)
        return y

class Sequential(nn.Sequential):
    
    def __init__(self, input_shape, output_shape, layers):
        self.input_shape = as_shape(input_shape)
        self.output_shape = as_shape(output_shape)
        super().__init__(*layers)       

class Encoder(Sequential):
    
    def __init__(self, input_shape, latent_shape, n=16):
        input_shape = as_shape(input_shape)
        output_shape = as_shape(latent_shape)
        self.__n = n
        layers = [
            nn.Conv2d(input_shape[0], n, (6,6), 2), nn.LeakyReLU(),
            ResBlock2D(n, n), nn.LeakyReLU(), 
            nn.Conv2d(n, 2*n, (3,3), 1), nn.LeakyReLU(),
            nn.Conv2d(2*n, 2*n, (3,3), 1), nn.LeakyReLU(),
            nn.Conv2d(2*n, 2*n, (2,2), 2), nn.LeakyReLU(), # downsample
            ResBlock2D(2*n, 2*n), nn.LeakyReLU(), 
            nn.Conv2d(2*n, 4*n, (3,3), 1), nn.LeakyReLU(),
            nn.Conv2d(4*n, 4*n, (2,2), 2), nn.LeakyReLU(), # downsample
            nn.Conv2d(4*n, 4*n, (3,3), 1), nn.LeakyReLU(),
        ]
        view = View((4*n,6,6),-1)
        layers.append(view)
        layers.append(nn.Linear(view.output_shape[0], output_shape[0]))
        super().__init__(input_shape, output_shape, layers)

        
    def inverse(self):
        n = self.__n
        view = View(-1,(4*n,6,6))
        layers = [
            nn.LeakyReLU(),
            nn.Linear(self.output_shape[0], view.input_shape[0]), nn.LeakyReLU(),
            view,
            nn.ConvTranspose2d(4*n, 4*n, (3,3), 1), nn.LeakyReLU(),
            nn.ConvTranspose2d(4*n, 4*n, (2,2), 2), nn.LeakyReLU(), 
            nn.ConvTranspose2d(4*n, 2*n, (3,3), 1), nn.LeakyReLU(),
            ResBlock2D(2*n, 2*n), nn.LeakyReLU(), 
            nn.ConvTranspose2d(2*n, 2*n, (2,2), 2), nn.LeakyReLU(), 
            nn.ConvTranspose2d(2*n, 2*n, (3,3), 1), nn.LeakyReLU(),
            nn.ConvTranspose2d(2*n, n, (3,3), 1), nn.LeakyReLU(),
            ResBlock2D(n, n), nn.LeakyReLU(),
            nn.ConvTranspose2d(n, self.input_shape[0], (6,6), 2)
        ]
        return Sequential(self.output_shape, self.input_shape, layers)    



class ResBlock2D(nn.Module):

    def __init__(self, in_channel, channel):
        super(ResBlock2D, self).__init__()
        self.in_channel = in_channel
        self.channel = channel

        self.c1 = nn.Conv2d(in_channel, channel, 3, 1, 1)
        self.r1 = nn.ReLU(inplace=True)
        self.c2 = nn.Conv2d(channel, in_channel, 1)
        
    def forward(self, x):
        x_ = self.c2(self.r1(self.c1(x)))
        x_ += x
        return x_

    def inverse(self, *args, share_weights=False, **kwargs):
        return ResBlock2D(self.in_channel, self.channel) # identity inverse...

def as_shape(x):
    if isinstance(x, tuple):
        return x
    elif isinstance(x, list):
        return tuple(x)
    elif isinstance(x, int):
        return (x,)
    else:
        raise ValueError(f"Invalid shape {x}.")
        
class View(nn.Module):
    """ Module that creates a view of an input tensor. """

    def __init__(self, input_shape, output_shape):
        super(View, self).__init__()
        def infer_shape(x, y): # x contains a -1
            assert not -1 in y
            t_y, t_x = np.prod(y), - np.prod(x)
            assert t_y % t_x == 0 # shapes are incompatible...
            x = list(x)
            x[x.index(-1)] = t_y // t_x
            return as_shape(x)

        self.input_shape = as_shape(input_shape)
        self.output_shape = as_shape(output_shape)

        # infer -1 in shape
        if -1 in self.output_shape:
            self.output_shape = infer_shape(self.output_shape, self.input_shape)
        if -1 in self.input_shape:
            self.input_shape = infer_shape(self.input_shape, self.output_shape)

    def forward(self, x):
        return x.view(x.shape[0], *self.output_shape)

    def __str__(self):
        attrbs = "{0}->{1}".format(self.input_shape, self.output_shape)
        return "{0}({1})".format(self.__class__.__name__, attrbs)

    def __repr__(self):
        return str(self)

    def shape(self, *args, **kwargs):
        return self.output_shape

    def inverse(self, **kwargs):
        return View(self.output_shape, self.input_shape)

class Flatten(View):
    """ A Module that creates a flat view of a tensor."""

    def __init__(self, input_shape):
        super().__init__(input_shape, (-1,))
        

def distance(fun):
    def decorator(x1, x2=None):
        if x2 is None:
            x2 = x1
        dist = fun(x1, x2)
        assert len(dist.shape) == 2
        assert dist.shape[0] == x1.shape[0]
        assert dist.shape[1] == x2.shape[0]
        return dist
    return decorator

@distance
def L22(x1, x2): # Squared L2
    n_dif = x1.unsqueeze(1) - x2.unsqueeze(0)
    return torch.sum(n_dif * n_dif, -1)

class PairedTripletLoss:

    def __init__(self, distance=L22, margin=0.2):
        self.distance = distance
        self.margin = margin

    def __call__(self, x1, x2): 
        """
            Pairs (x1, x2)_i share the same label. All others will be treated as negative examples.
            This makes training very efficient, but at the cost of ensuring that inputs are of the correct form. 
            Best used when many labels are present - or for time series data. 
        """
        d = self.distance(x1, x2)
        xp = torch.diag(d).unsqueeze(1)
        xn = d # careful with the diagonal?

        # is doesnt matter if xp is included in xn as xp_i - xn_i = 0, the original inequality is satisfied, the loss will be 0.
        # it may be more expensive to remove these values than to just compute as below.
        xf = xp.unsqueeze(2) - xn #should only consist of only ||A - P|| - ||A - N|| [batch_size x batch_size x k]
        
        xf = F.relu(xf + self.margin)
        return xf.mean()
    
