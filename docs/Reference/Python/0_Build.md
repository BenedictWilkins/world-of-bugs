# Build

<a id="worldofbugs.build"></a>

## Module worldofbugs.build

Build all of the unity environments available in `UnityProject`. Environment builds are placed in `worldofbugs/builds`. 
Building requires unity editor version `UNITY_VERSION` and the unity mlagents plugin `ML_AGENTS_VERSION`, 
for details see [here](https://github.com/Unity-Technologies/ml-agents). Currently a build corresponds 
to a particular scene and can only be built for linux.
Alternatively, you can download builds from [here](https://www.kaggle.com/benedictwilkinsai/world-of-bugs). 
Building requires the following: 
```
UNITY_VERSION = "2020.3.25f1"
ML_AGENTS_VERSION = "2.1.0-exp1 (Preview)"
```

