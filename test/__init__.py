from worldofbugs.dataset import dataset, load
from worldofbugs.utils import get_unity_environments, make

#dataset("World-v1", workers=20, n_episodes=200, max_episode_length=1000)
#dataset("World-v2", workers=10, n_episodes=200, max_episode_length=1000)


print(get_unity_environments())
env = make("WorldTest-v2", display_width=84*4, display_height=84*4)
print(env.unwrapped)
env.enable_bug("ZFighting")
env.reset()

for i in range(50):
    env.step(3)

env.disable_bug("ZFighting")
env.reset()

for i in range(100):
    env.step(3)
