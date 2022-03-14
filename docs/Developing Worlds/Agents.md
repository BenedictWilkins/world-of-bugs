# Agents


# Structure of an Agent

The default structure of an agent is derived from Unity's MLAgents package abstractions, including ISensor, IActuator, and Agent. 




-------- 

# Actions and Observations

The WOB Python API is the same whether using a standalone build or the unity editor. WOB follows [OpenAI Gym](https://gym.openai.com/) for a single agent with some extras to help set up environments in Unity. 

The `step` method of the environment is where actions are taken and observations are collected, its usage looks as follows.

{{< card-code lang="Python">}} observation, reward, done, info = env.step(action) {{< /card-code >}}

The `observation` contains visual data