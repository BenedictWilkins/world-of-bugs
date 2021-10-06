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

from . import utils
DATASET_NAME = "dataset"
DATASET_EXT = ".hdf5"

def dataset(env_id, policy=None, n_episodes=10, max_episode_length=100, workers=10):
    """ Create a new dataset from a unity build.

    Args:
        env_id (str): environment ID of the unity environment (should match the build folder name/build name.)
        policy (function, optional): policy used to generate data. Defaults to None.
        n_episodes (int, optional): number of episodes to generate. Defaults to 10.
        max_episode_length (int, optional): max length of an episode (longer episodes will be cut to fit). Defaults to 100.
        workers (int, optional): numbe of (ray) workers to use. Defaults to 10.

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

    env_f = lambda : utils.make(env_id, worker=os.getpid())
    if workers > 1:
        import ray
        ray.init()
        workers = [EpisodesWorker.remote(env_f, policy=policy, n_episodes = int(n_episodes // workers), max_episode_length=max_episode_length) for _ in range(workers)]
        iter = ray.util.iter.from_actors(workers).gather_async()
    else:
        iter = Episodes(env_f, policy=policy, n_episodes=n_episodes, max_episode_length=max_episode_length) 
    
    with h5py.File(path, "w") as f:
        for i, (state, action, reward, done) in enumerate(tqdm(iter, total=n_episodes)):
            g = f.create_group(f"ep-{str(i).zfill(4)}")
            g.create_dataset('state', data=state)
            g.create_dataset('action', data=action) 
            g.create_dataset('reward', data=reward)
            g.create_dataset('done', data=done)

def load(env_id, dataset_id=0):
    _, ex_path = utils._get_unity_environment_info(env_id)
    path, _ = os.path.split(ex_path)
    path = os.path.join(path, 'dataset')
    path = os.path.join(path, f"{DATASET_NAME}-{dataset_id}{DATASET_EXT}")
    with h5py.File(path, "r") as f:
        for k in sorted(f.keys()):
            yield f[k]['state'][...], f[k]['action'][...], f[k]['reward'][...], f[k]['done'][...]

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
        state, action, reward, done = np.stack(state), np.stack(action), np.stack(reward), np.stack(done).astype(np.uint8)
        return state, action, reward, done

    def __len__(self):
        return self.n_episodes

class GymIterator:

    def __init__(self, env, policy=None):
        self.env = env
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
