from worldofbugs.dataset import dataset, load
from worldofbugs.utils import get_unity_environments

#dataset("World-v1", workers=20, n_episodes=200, max_episode_length=1000)
dataset("World-v2", workers=10, n_episodes=200, max_episode_length=1000)