---
title: ""
date: 2021-10-15T12:29:06+01:00
draft: false
---

### Texture Missing

When a texture is missing from an object a shader will typically resort to rendering it in one colour, in the example below, pink, to signify an issue.

{{< gallery dir="/image/TextureMissing" >}} 

### Texture Corruption

A texture may render incorrectly/become corrupte for various reasons, for example, offsets incorrectly set, the UV map is incorrect or other shader related issues. Similar visually jaring effects may also be due to lighting problems.

{{< gallery dir="/image/TextureCorrupt" >}} 

### Geometry Corruption

### Geometry Clipping

###  Z-fighting

Z-Fighting happens when two sufaces have the same depth (z). The shader does not know which to show first and this results in a mixing of textures as shown below. A flickering effect may also occur during a view point change.

{{< gallery dir="/image/ZFighting" >}} 


### Z-Clipping

Rendering pipelines often have layers, layers are placed in a queue and rendered in order, new pixels replace or are blended with old pixels in subsequent layers. If an object is placed in the wrong layer, it can cause Z-clipping, the object is rendered on top of other objects leading to a confusing spatial effect like the one seen below.

{{< gallery dir="/image/ZClipping" >}} 

### Camera Clipping

For efficiency reasons, rendering pipelines cull geometry that is out of view. Cameras are a useful abstraction that are used by many engines to describe what can be seen on screen, the view frustum is used to cull unseen geometry. If this frustum is too small, some geometry that should be rendered isn't. In the example below, the near clipping plane has been set too far away from the player. At certain view points, when close to an object some of its geometry is culled leading to the "see through" effect seen below. In some cicumstances this bug allows players to see into areas they shouldnt be able to (looking through walls/floors). 

{{< gallery dir="/image/CameraClip" >}} 

### Black Screen

### Screen Tearing

# Control

### Unresponsive

### Action Failed

### Action Incorrect

# Game Play / Physics

### Invisible Wall

### Hole in Map
