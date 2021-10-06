from worldofbugs.dataset import dataset, load

dataset("World-v2", workers=10, n_episodes=200, max_episode_length=1000)
#for ep in load("World-v1", 1):
#    print("EP", ep[0].shape)