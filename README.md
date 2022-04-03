# WorldOfBugs

World of Bugs is a test plaform for Automated Bug Detection (ABD) research. 

## READER NOTE:

Major refactoring is happening in the dev branch of this repositroy in preparation for version 1.0 release. Any documentation is in the process of being updated for this branch.

### Documentation

Documentation can be found [here](https://benedictwilkins.github.io/world-of-bugs/).

### Datasets

Datasets have been created using our test environment, a pre-built version can be found [here](https://www.kaggle.com/benedictwilkinsai/world-of-bugs).

* Training data is available for download [here](https://www.kaggle.com/benedictwilkinsai/world-of-bugs-normal) and [here](TODO)
* Test data is avaliable for download [here](https://www.kaggle.com/benedictwilkinsai/world-of-bugs-test)

### Experiments

Experiments on the initial version of the test environment (World-v1) can be found in [this](https://github.com/BenedictWilkins/world-of-bugs-experiments) repository.



### Paper

LINKS COMING SOON



## Bug Zoo

Below is a list of the currently implemented bugs in WOB, the list is growing!

| Name/Description | ----------Example---------- | 
| ---- | ----- |
| **Boundary Hole:** A specific kind of Geometry Clipping bug in which the player falls through the terrain due to gravity, or is able to walk out of the bounds of play. _Implementation_: Disable the collision box of a boundary object/terrain. | <img src="https://github.com/BenedictWilkins/WorldOfBugs/blob/gh-pages/static/image/bugs/BoundaryHole.png?raw=True" width=256 style="border: 1px solid black"> |
| **Black Screen:** A black screen usually happens when there is an issue with the rendering pipeline, leading to a screen that is black (nothing is rendered). _Implementation_: Render a black texture to the screen in a post processing step. | <img src="https://github.com/BenedictWilkins/WorldOfBugs/blob/gh-pages/static/image/bugs/BlackScreen.png?raw=True" width=256 style="border: 1px solid black"> |
| **Camera Clipping:** For efficiency reasons, rendering pipelines cull geometry that is out of view. Cameras are a useful abstraction that are used by many engines to describe what can be seen on screen, the cameras view frustum is used to cull unseen geometry. If this frustum is too small, some geometry that should be rendered is not. In the example below, the near clipping plane has been set too far away from the player. At certain view points, when close to an object some of its geometry is culled leading to the “see through” effect seen below. In some circumstances this bug allows players to see into areas they shouldn't be able to (looking through walls/floors). This bug may only manifest itself with certain objects, particularly ones that have an odd shape (non-convex or pointy) and so it may not be immediately obvious to a developer that there is an issue. _Implementation_: Modify the near clipping plane. | <img src="https://github.com/BenedictWilkins/WorldOfBugs/blob/gh-pages/static/image/bugs/CameraClipping.png?raw=True" width=256  style="border: 1px solid black"> |
| **Geometry Clipping:** A bug that manifests in a visually similar way to camera clipping, but has a different cause. If collisions between the player and an object are not computed correctly, the player may be able walk inside the object and so is able to see inside it. Note: similar collision based bugs between two (non-player) objects, or _ghost objects_ also manifest visually, this has not been explored in our work but would be an interesting case to study. _Implementation_: Disable the collision box of an object. | <img src="https://github.com/BenedictWilkins/WorldOfBugs/blob/gh-pages/static/image/bugs/GeometryClipping.png?raw=True" width=256  style="border: 1px solid black"> |
| **Geometry Corruption:** Similarly to Texture Corruption, object geometry can also become corrupted. A geometry is corrupted when some of its vertices are incorrectly placed relative to the other vertices. They tend to happen during animations that depend on physics with flexible/dynamic geometries, but can also happen with static geometry (although this is less common). _Implementation_: randomly modifies the vertex positions of a chosen game object geometry over time. | <img src="https://github.com/BenedictWilkins/WorldOfBugs/blob/gh-pages/static/image/bugs/GeometryCorruption.png?raw=True" width=256 style="border: 1px solid black"> |
| **Screen Tear:** Screen tearing happens when a monitors refresh rate doesn't match the GPU frame rate. This leads to information multiple frames being rendered to the screen as a single frame, leading to a tearing effect. Although typically a hardware problem, it represents an interesting and often subtle visual issue. _Implementation_: Record previous frames and render them embedded in the current frame. | <img src="https://github.com/BenedictWilkins/WorldOfBugs/blob/gh-pages/static/image/bugs/ScreenTear.png?raw=True" width=256 style="border: 1px solid black"> |
| **Texture Corruption:** A texture may render incorrectly/become corrupt for various reasons, for example, when texture offsets incorrectly set, the UV map is incorrect or as a result of shader related issues. Similar effects may also be due to lighting problems, for example if the vertex normals are incorrectly set.  _Implementation:_ the UV map/offsets are modified to corrupt the texture. | <img src="https://github.com/BenedictWilkins/WorldOfBugs/blob/gh-pages/static/image/bugs/TextureCorruption.png?raw=True" width=256 style="border: 1px solid black"> |
| **Texture Missing:** When a texture is missing from an object, which may happen if a developer forgets to set the texture, its missing/inaccessible in the file system or otherwise, the shader responsible for rendering the texture will typically resort to rendering the object it in one colour (typically bright pink) or resort to a default texture. _Implementation_: remove the texture from a chosen game object. | <img src="https://github.com/BenedictWilkins/WorldOfBugs/blob/gh-pages/static/image/bugs/TextureMissing.png?raw=True" width=256 style="border: 1px solid black"> |
| **Z-Clipping:** Rendering pipelines often have layers, layers contain geometry to be rendered and are queued and rendered in order. Pixels in the layers rendered subsequently are replaced or are blended with pixels in previous layers. If an object geometry is placed in the wrong layer, it may be rendered on top of others leading to some strange spatial effects (which may or may not be desirable). _Implementation_: places a chosen game object in a layer that is always rendered last. | <img src="https://github.com/BenedictWilkins/WorldOfBugs/blob/gh-pages/static/image/bugs/ZClipping.png?raw=True" width=256 style="border: 1px solid black"> |
| **Z-Fighting:** Z-Fighting happens when two surfaces have the same depth (z). The renderer/shader does not know which to show first and this results in a mixing of textures from the two surfaces. A flickering effect may also occur during when the players view shifts. _Implementation_: copies a game object, modifies its texture and places it exactly at the position of the original. | <img src="https://github.com/BenedictWilkins/WorldOfBugs/blob/gh-pages/static/image/bugs/ZFighting.png?raw=True" width=256 style="border: 1px solid black"> |
| **Invisible Wall:** COMING SOON | COMING SOON |
| **Unresponsive:**   COMING SOON | COMING SOON |
| **Freezing/Lag:**   COMING SOON | COMING SOON |



