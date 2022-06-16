# environment.sidechannel

<a id="worldofbugs.environment.sidechannel"></a>

## Module sidechannel

Side channels are used to communicate data to Unity and are a feature of Unity MLAgents.
`worldofbug` defines two useful, one for logging, and one for configuration.

<a id="worldofbugs.environment.sidechannel.UnityLogChannel"></a>

### Class UnityLogChannel

```python
class UnityLogChannel(SideChannel)
```

Logging SideChannel, reads log messages from Unity and prints them to stdout.

<a id="worldofbugs.environment.sidechannel.UnityConfigChannel"></a>

### Class UnityConfigChannel

```python
class UnityConfigChannel(SideChannel)
```

Configuration SideChannels, sends config information to a Unity environment. For API details see: [TODO]()

<a id="worldofbugs.environment.sidechannel.UnityConfigChannel.write"></a>

#### write

```python
def write(msg)
```

Write to the side channel.

**Arguments**:

- `msg` _str_ - message to write

