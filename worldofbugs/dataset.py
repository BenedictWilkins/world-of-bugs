#!/usr/bin/env python
# -*- coding: utf-8 -*-
"""
    Created on 05-10-2021 14:58:51

    [Description]
"""
__author__ = "Benedict Wilkins"
__email__ = "benrjw@gmail.com"
__status__ = "Development"

import os
import glob
import ray
import itertools
import h5py
import numpy as np

from tqdm.auto import tqdm
from types import SimpleNamespace

from . import utils
DATASET_NAME = "dataset"
DATASET_EXT = ".hdf5"

def load(env_id, dataset_id=0):
    _, ex_path = utils._get_unity_environment_info(env_id)
    path, _ = os.path.split(ex_path)
    path = os.path.join(path, 'dataset')
    path = os.path.join(path, f"{DATASET_NAME}-{dataset_id}{DATASET_EXT}")
    with h5py.File(path, "r") as f:
        for k in sorted(f.keys()):
            episode = f[k]
            yield SimpleNamespace(**{j:episode[j][...] for j in episode.keys()})

def dataset(env_id, policy=None, n_episodes=10, max_episode_length=100, workers=10, bugs=[]):
    """ Create a new dataset from a unity build.

    Args:
        env_id (str): environment ID of the unity environment (should match the build folder name/build name.)
        policy (function, optional): policy used to generate data. Defaults to None.
        n_episodes (int, optional): number of episodes to generate. Defaults to 10.
        max_episode_length (int, optional): max length of an episode (longer episodes will be cut to fit). Defaults to 100.
        workers (int, optional): number of (ray) workers to use. Defaults to 10.
        bugs (list): list of bugs to enable in the environment
    Raises:
        FileExistsError: if a dataset file already exists
    """
    _, ex_path = utils._get_unity_environment_info(env_id)
    path, _ = os.path.split(ex_path)
    path = os.path.join(path, 'dataset')
    if not os.path.exists(path):
        os.mkdir(path)
    dataset_files = [(f) for f in glob.glob(os.path.join(path, f"{DATASET_NAME}-*{DATASET_EXT}"))]
    path = os.path.join(path, f"{DATASET_NAME}-{len(dataset_files)}{DATASET_EXT}")
    
    if os.path.exists(path):
        raise FileExistsError(f"{path} already exists, failed to create a new dataset.")

    def make_environment(): 
        env =  utils.make(env_id, worker=os.getpid())
        for bug in bugs:
            env.enable_bug(bug)
        return env
   
    if workers > 1:
        import ray
        ray.init()
        workers = [EpisodesWorker.remote(make_environment, policy=policy, n_episodes = int(n_episodes // workers), max_episode_length=max_episode_length) for _ in range(workers)]
        iter = ray.util.iter.from_actors(workers).gather_async()
    else:
        iter = Episodes(make_environment, policy=policy, n_episodes=n_episodes, max_episode_length=max_episode_length) 
    
    with h5py.File(path, "w") as f:
        for i, data in enumerate(tqdm(iter, total=n_episodes)):
            g = f.create_group(f"ep-{str(i).zfill(4)}")
            for k,v in data.items():
                g.create_dataset(k.lower(), data=v)

class Episodes:

    def __init__(self, env_f, policy=None, n_episodes=1, max_episode_length=1000):   
        super().__init__()
        self.env = env_f()
        self.policy = policy
        self.n_episodes = n_episodes
        self.max_episode_length = max_episode_length
        self.iterator = GymIterator(self.env, self.policy)

    def __iter__(self):
        if self.n_episodes > 0:
            iter = itertools.repeat(self.episode, self.n_episodes)
        else:
            iter = itertools.repeat(self.episode)
        for f in iter:
            yield f()

    def episode(self):
        iterator = itertools.islice(self.iterator, 0, self.max_episode_length)
        state, action, reward, done = zip(*iterator)
        sensor_keys = state[0].keys()
        data = {k:np.stack([d[k] for d in state]) for k in sensor_keys} # maybe there is a more efficient way.. TODO
        info = data.get('info', None)
        if info is not None:
            action = info[:,0] # if we are using a python policy this will be the same as action anyway...
        else:
            action = np.stack(action)
        data['action'] = action.astype(np.int64)
        data['reward'] = np.stack(reward).astype(np.float32)
        data['done'] = np.stack(done).astype(np.uint8)
        return data

    def __len__(self):
        return self.n_episodes

class GymIterator:

    def __init__(self, env, policy=None):
        self.env = env
        self.env.reset() # reset once to avoid an initial black frame...
        if policy is None:
            policy = lambda *args, **kwargs: env.action_space.sample()
        self.policy = policy
        
    def __iter__(self):
        state = self.env.reset()
        done = False
        while not done:
            action = self.policy(state)
            next_state, reward, done, info = self.env.step(action)
            yield state, action, reward, done
            state = next_state

@ray.remote
class EpisodesWorker(ray.util.iter.ParallelIteratorWorker):
    def __init__(self, *args, **kwargs):
        super().__init__(Episodes(*args, **kwargs), False)
