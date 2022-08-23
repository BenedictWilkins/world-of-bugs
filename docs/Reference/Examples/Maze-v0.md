# Maze-v0

Maze-v0 is an environment that has a focus on exploration. The agent is tasked with navigating a maze. Various bugs that allow the agent to cheat have been enabled in this environment.

<img src="../imgs/Maze-v0.png"  width=50% style="display:block; margin-left:auto; margin-right:auto; margin-bottom:1rem;">

=== "Video"

    !!! todo "VIDEO COMING SOON"


=== "Bugs"

    ### Invalid Information Access

    This bug allows the player to obtain information that would not otherwise have by looking through walls.

    <img src="../imgs/Maze-v0-InvalidInformationAccess.png"  width=50% style="display:block; margin-left:auto; margin-right:auto; margin-bottom:1rem;">

    ### Invalid Shortcut

    This bug allows the player to pass through some of the walls in the maze. The walls are selected randomly in each run.

    <img src="../imgs/Maze-v0-InvalidShortcut.png"  width=50% style="display:block; margin-left:auto; margin-right:auto; margin-bottom:1rem;">

    ### Out of Bounds

    This bug allows the player to escape the playable area. The player will only be able to escape for a moment before "falling out of the world" and reseting.

    <img src="../imgs/Maze-v0-OutOfBounds.png"  width=50% style="display:block; margin-left:auto; margin-right:auto; margin-bottom:1rem;">

=== "Action Space"

    | Action      | Index |  Type | Description                          |
    | :---------- | :---  | :---  |:----------------------------------- |
    | `IDLE`            | 0 | Discrete |  Does nothing.                             |
    | `FORWARD`         | 1 | Discrete | Moves the agent in the facing direction.  |
    | `ROTATE_LEFT`     | 2 | Discrete | Turns the agent left.                     |
    | `ROTATE_RIGHT`    | 3 | Discrete | Turns the agent right.                    |


=== "Observation Space"

    | Observation  |  Type | Description |
    |  |  |  |
    | `Observation`         | `(3,84,84) float` | First person view of the environment.  |
    | `Mask`                | `(3,84,84) float` | Masked first person view of the environment highlighting bugged regions. |
    | `Position`            | `(3,) float` | Position of the agent. |
    | `Rotation`            | `(3,) float` | Rotation of the agent. |
    | `Action`              | `(1,) float` | The most recent action taken by the agent. |

## Quick Start

Download the [standalone build](https://github.com/BenedictWilkins/world-of-bugs/releases/tag/test-build) and place in `~/Downloads/builds'.

```
import worldofbugs

worldofbugs.utils.BuildResolver.search_paths += "~/Downloads/builds"   # add downloaded/builds to path
print(worldofbugs.utils.BuildResolver.search_paths)                     # list all search paths
print(worldofbugs.utils.BuildResolver.builds)                           # list all avaliable environments

# make the environment
env = worldofbugs.make('WOB/Maze-v0')
env.set_agent_behaviour('HeuristicNavMesh')

env.reset()
for i in range(1000):
  obs, reward, done, info = env.step(0)
  env.render()

```