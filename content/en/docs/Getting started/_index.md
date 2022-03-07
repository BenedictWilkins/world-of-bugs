---
title: "Quick Start Guide"
linkTitle: "Quick Start Guide"
weight: 2
---

1. Clone the github repository: 

```git clone https://github.com/BenedictWilkins/world-of-bugs.git```

2. While in the same directory, install with: 
   
  ```python -m pip install world-of-bugs```

3. Download the latest build for your system [here](https://github.com/BenedictWilkins/world-of-bugs/releases/tag/Release).

4. Extract the build files to a directory of your choice, for example `~/Downloads/builds/Standalone-Linux-World-v1`.

5. To run with the default build settings:

{{< card-code header="MyScript.py" lang="Python">}}import worldofbugs

# add downloaded builds to path
worldofbugs.utils.BuildResolver.path += "~/Downloads/builds/"
print(worldofbugs.utils.BuildResolver.get_builds()) # sanity check, list all avaliable environments

# make the environment
env = worldofbugs.utils.make('World-v1') 

env.reset()
for i in range(1000): # advance simulation 1000 steps
  env.step(env.action_space.sample()) # take a random action
  env.render() #render the game screen, requires pygame installation
{{< /card-code  >}}

If everything worked correctly you should see a printout like: 

``` 
>>> [~/Downloads/builds/Standalone-Linux-World-v1/World-v1.x86_64]
``` 
and if you have pygame installed (`pip install pygame`) something like the following: 