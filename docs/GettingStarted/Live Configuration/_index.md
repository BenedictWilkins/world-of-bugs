An environment can be configured using the [WOB environment API](TODO). 

#### Configuring Bugs

A bug can be enabled using the `enable_bug` method, for example: `env.enable_bug("MissingTexture")`

A bug can be disabled using the `disable_bug` method, for example: `env.disable_bug("TextureCorruption")`

Bugs names a derived from their type name as it appears in Unity, a full list of bugs can be found 

#### Configuring Agent Behaviour

An agents behvaiour can be changed using the `set_agent_behaviour`, for example: `env.set_agent_behaviour("Manual")`. 

Setting the agents behaviour will enable heuristic mode. In this mode, actions will be selected by the given Unity script rather than by Python, i.e. the action in `env.step(action)` will be ignored. 

### Advanced




## Logging

By default all messages logged with Unity's default logging system, `Debug.Log` are also displayed in the Python console. 