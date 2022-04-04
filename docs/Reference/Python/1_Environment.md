# Environment

<a id="worldofbugs.environment"></a>

## Module worldofbugs.environment

The world of bugs environment API. For most usecases [`make`](#worldofbugs.environment.make) is sufficient.

<a id="worldofbugs.environment.make"></a>

### worldofbugs.environment.make

```python
def make(env_id: str,
         worker: int = 0,
         time_scale: float = 1.0,
         log_folder: str = None,
         debug: bool = True)
```

Make a `worldofbugs` environment. This is different from the default `gym.make` function as it hooks directly into the underlying Unity environments. This allows one to connect to the a Unity Editor as well as any `worldofbugs` build that may or may not be registered with `gym`. Any builds that exist in the default builds directory for the `worldofbugs` package may be loaded using `gym.make`.

**Arguments**:

- `env_id` _str_ - unique environment ID, follows the `gym` format e.g. `WoB/World-v1`. A value of `None` will attempt to connect to a Unity Editor instance.
- `worker` _int, optional_ - Worker process to run the environment. This allows multiple environments to run in parallel. Defaults to 0.
- `time_scale` _float, optional_ - Simulation speed. WARNING: this can break the game physics, only change if you know what you are doing. Defaults to 1.0.
- `log_folder` _str, optional_ - directoy to log to. Defaults to None.
- `debug` _bool, optional_ - Get logging messages from unity program and write them to Python stdout. Defaults to True.
  

**Returns**:

- `gym.Env` - the `worldofbugs` environment.

