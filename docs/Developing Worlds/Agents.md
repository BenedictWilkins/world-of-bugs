# Agents

WOB adds some additional features to Unity's [ML-Agents package](https://github.com/Unity-Technologies/ml-agents). For details on how to implement your own agent take a look at their documentation. Otherwise, continue to find out how to make use of what WOB has to offer.

## Creating an Agent

An agent is built from a number of components, many of which derive from ML-Agents, they are outlined below.


## Sensors

A WOB agent by default uses two key sensors:

* The main camera - which renders the view of the environment as would otherwise be seen by a human player.

* The bug mask camera - which renders a mask over this main camera showing where bugs are present if any.

Rather than rendering to the screen, each camera renders to a `RenderTexture`. The `RenderTexture` is used by the agent's `RenderTextureSensor`. 

In addition to the main and bug mask sensors, other sensors may be added to the agent. One straight forward way of doing this is via the `Observable` attribute. When properties are decorated with the attribute, their value will be retrieved as part of the agent's observation. 

```cs
public class MyAgent : WorldOfBugs.Agent {
    
    [Observable("Position")]
    public Vector3 Position { get { return gameObject.transform.position; }}
    
    [Observable("Rotation")]
    public Vector3 Rotation { get { return gameObject.transform.eulerAngles; }}
}
```

The agents observations can be found in Python as follows:

```Python
observation, reward, done, info = env.step(action)

observation 
>> np.array( ... ) // main camera view

info 
>> Position : np.array( ... ) // position, shape (3,) 
>> Rotation : np.array( ... ) // rotation, shape (3,)
>> Mask :     np.array( ... ) // bug mask view
```

Observations other than the main camera observation are part of `info` to adhere to OpenAI gym's API guidelines.




## Actuators

The actions an agent can perform are usually defined as part of the agents `BehaviourParameters` (see ML-Agents documentation for details), however WOB provides a simpler mechanism using `ActionAttributes`. 

`Action` attribute is similar to `Observable` but decorates actions rather than observations. When defining a new `Agent` class, the `Action` attribute can be added to methods as follows:

```cs
public class MyAgent : WorldOfBugs.Agent {
    
    [Action]
    public void none() {} // does nothing

    [Action]
    public void forward() { // move the agent in the direction its facing
        Vector3 movement = gameObject.transform.forward * Time.deltaTime * MovementSpeed;
        gameObject.transform.Translate(movement, Space.World);
    }

    [Action]
    public void rotate_left() { // rotate to face left
        gameObject.transform.Rotate(new Vector3(0, -1 * Time.deltaTime * AngularSpeed, 0));
    }

    [Action]
    public void rotate_right() { // rotate to face right
        gameObject.transform.Rotate(new Vector3(0, 1 * Time.deltaTime * AngularSpeed, 0));
    }
}
```

This will automatically configure the agent and define a discrete action space containing four actions. Actions are ordered by the appearance in the source file. Taking the action `forward` in Python corresponds to `env.step(1)`. 

## Behaviours & Heuristics

Both Python and C# can be used to create agent behaviours. 

=== "Python"
    WOB uses OpenAI gym to interface with the environment, behaviours need only provide their actions to `env.step`. 

=== "C#" 
    In C# a behaviour may be written as a heuristic, or using a neural network. To explore this further see the ML-Agents documentation.

### Gathering Observations with Python

Sometimes it is convenient to connect to an environment with the gym interface but to make use of a C# behaviour. ML-Agents does not currently support this, but it can be achieved by setting behaviours using the WOB API. The actions taken by the C# behaviour can be obtained by observing an agent property as follows: 

```cs
[Observable("Action")]
public int Action { get { return GetStoredActionBuffers().DiscreteActions[0]; }}
```


