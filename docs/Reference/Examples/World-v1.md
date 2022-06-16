# World-v1

World-v1 is a simple environment that has been built to test approaches to the bug identification problem. Exploration is trivial as the agent is situated in a small static room containing two cubes.

=== "Video"

    Demo of the built-in navigation agent exploring and uncovering various bugs.

    <iframe width="560" height="315" src="https://www.youtube.com/embed/UZbD8SivkAA?rel=0&modestbranding=1&autohide=1&mute=1&showinfo=0" title="YouTube video player" frameborder="0" allow="accelerometer; autoplay; clipboard-write; encrypted-media; gyroscope; picture-in-picture" allowfullscreen style="border:none;overflow:hidden;display:block;margin:0 auto;"></iframe>

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
    | `Mask`                |`(3,84,84) float` | Masked first person view of the environment highlighting bugged regions. |
    | `Position`            | `(3,) float` | Position of the agent. |
    | `Rotation`            | `(3,) float` | Rotation of the agent. |
    | `Action`              | `(1,) float` | The most recent action taken by the agent. |
