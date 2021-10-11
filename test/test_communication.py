
from mlagents_envs.environment import UnityEnvironment
from mlagents_envs.side_channel.side_channel import SideChannel, IncomingMessage, OutgoingMessage
import uuid


from worldofbugs.environment import BuggedUnityEnvironment

# We start the communication with the Unity Editor and pass the string_log side channel as input
env = BuggedUnityEnvironment()
env.enable_bug("ZFighting")
env.reset()

group_name = list(env.behavior_specs.keys())[0]  # Get the first group_name
group_spec = env.behavior_specs[group_name]
for i in range(1000):
    decision_steps, terminal_steps = env.get_steps(group_name)
    env.step()  # Move the simulation forward

env.close()