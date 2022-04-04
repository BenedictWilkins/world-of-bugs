# Environment.sidechannel

<a id="worldofbugs.environment.sidechannel"></a>

## Module worldofbugs.environment.sidechannel

Side channels are used to communicate data to Unity and are a feature of Unity MLAgents.
`worldofbug` defines two useful, one for logging, and one for configuration.

<a id="worldofbugs.environment.sidechannel.UnityLogChannel"></a>

### Class worldofbugs.environment.sidechannel.UnityLogChannel

```python
class UnityLogChannel(SideChannel)
```

Logging SideChannel, reads log messages from Unity and prints them to stdout.

<a id="worldofbugs.environment.sidechannel.UnityConfigChannel"></a>

### Class worldofbugs.environment.sidechannel.UnityConfigChannel

```python
class UnityConfigChannel(SideChannel)
```

Configuration SideChannels, sends config information to a Unity environment. For API details see: [TODO]()

