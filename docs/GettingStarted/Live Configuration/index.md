# Live Configuration

`worldofbugs` environment supports live configuration. Bugs can be configured at runtime via a simple Python API, outlined below.

## Configuring Bugs

A bug can be enabled using the `enable_bug` method, for example: 

```
env.enable_bug("MissingTexture")
```

A bug can be disabled using the `disable_bug` method, for example: 

```
env.disable_bug("MissingTexture")
```

Bug names a derived from their type name as it appears in Unity. A full list of bugs can be found [here](../../BugZoo.md). It is up to the environment which bugs it implements, so not all environments will implement all bugs.

## Configuring Agent Behaviour

!!! todo
    this is now out of date and will be updated soon.

An agent's behaviour can be changed using the `set_agent_behaviour`, for example: `env.set_agent_behaviour("PolicyPython")`. 

Setting the behaviour to `"PolicyPython"` will enable the current python policy, the agent will take actions received via `env.step`. Otherwise, a heuristic behaviour defined in unity will be used, any python actions will be ignored. The actual action taken can be retrieved by inspecting `info` returned by `env.step` as follows:

```python
    state, reward, done, info = env.step(0)
    info['Action']
```

Again, it is up to the environment (and in particular the agent) to define which heuristic behaviours are available. Standard behaviours include: 

* `"PolicyPython"` - use the python behaviour as described above.
* `"<ENV_NAME>HeuristicManual"` - use keyboard/mouse input.
* `"<ENV_NAME>HeuristicNavMesh"` - use Unity's NavMesh system.

## Advanced

MORE INFO COMING SOON

## Logging

By default, all messages logged with Unity's default logging system, `Debug.Log` are also displayed in the Python console. This can be configured when an environment is created by specifying `debug = True`, i.e. `worldofbugs.make("World-v1", debug=True)`.