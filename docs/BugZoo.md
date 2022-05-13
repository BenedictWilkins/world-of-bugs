# Bug Zoo

Below is a list of the bugs currently implemented in WOB, the list is growing! 

-------------------------- 

## Boundary Hole

=== "Description"
    A specific kind of Geometry Clipping bug in which the player falls through the terrain due to gravity, or is able to walk out of the bounds of play.

=== "Implementation"
    Disable the collision box of a boundary object/terrain. 

<iframe width="560" height="315" src="https://www.youtube.com/embed/Oyj74pRgu4g?playlist=Oyj74pRgu4g&rel=0&loop=1&modestbranding=1&autohide=1&mute=1&showinfo=0" title="YouTube video player" frameborder="0" allowfullscreen style="border:none;overflow:hidden;display:block;margin:0 auto;"></iframe>



-------------------------- 

## Black Screen
=== "Description"
    A black screen usually happens when there is an issue with the rendering pipeline, leading to a screen that is black (nothing is rendered).
=== "Implementation"
    Render a black texture to the screen in a post processing step. 

<iframe width="560" height="315" src="https://www.youtube.com/embed/DQxEkIaTG5A?playlist=DQxEkIaTG5A&rel=0&loop=1&modestbranding=1&autohide=1&mute=1&showinfo=0" title="YouTube video player" frameborder="0" allow="accelerometer; autoplay; clipboard-write; encrypted-media; gyroscope; picture-in-picture" allowfullscreen style="border:none;overflow:hidden;display:block;margin:0 auto;"></iframe> 

-------------------------- 

## Camera Clipping
=== "Description"
    For efficiency reasons, rendering pipelines cull geometry that is out of view. Cameras are a useful abstraction that are used by many engines to describe what can be seen on screen, the cameras view frustum is used to cull unseen geometry. If this frustum is too small, some geometry that should be rendered is not. In the example below, the near clipping plane has been set too far away from the player. At certain view points, when close to an object some of its geometry is culled leading to the “see through” effect seen below. In some circumstances this bug allows players to see into areas they shouldn't be able to (looking through walls/floors). This bug may only manifest itself with certain objects, particularly ones that have an odd shape (non-convex or pointy) and so it may not be immediately obvious to a developer that there is an issue.
=== "Implementation"
    Modify the near clipping plane.  

<iframe width="560" height="315" src="https://www.youtube.com/embed/8FDb3p0PjTA?playlist=8FDb3p0PjTA&rel=0&loop=1&modestbranding=1&autohide=1&mute=1&showinfo=0" title="YouTube video player" frameborder="0" allow="accelerometer; autoplay; clipboard-write; encrypted-media; gyroscope; picture-in-picture" allowfullscreen style="border:none;overflow:hidden;display:block;margin:0 auto;"></iframe> 

-------------------------- 

## Geometry Clipping
=== "Description"
    A bug that manifests in a visually similar way to camera clipping, but has a different cause. If collisions between the player and an object are not computed correctly, the player may be able to walk inside the object and so is able to see inside it. Note: similar collision based bugs between two (non-player) objects, or _ghost objects_ also manifest visually, this has not been explored in our work but would be an interesting case to study.
=== "Implementation"
    Disable the collision box of an object. 

<iframe width="560" height="315" src="https://www.youtube.com/embed/4O3iNgFN-OA?playlist=4O3iNgFN-OA&rel=0&loop=1&modestbranding=1&autohide=1&mute=1&showinfo=0" title="YouTube video player" frameborder="0" allow="accelerometer; autoplay; clipboard-write; encrypted-media; gyroscope; picture-in-picture" allowfullscreen style="border:none;overflow:hidden;display:block;margin:0 auto;"></iframe> 

-------------------------- 


## Geometry Corruption 

=== "Description"
    Similarly to Texture Corruption, object geometry can also become corrupted. A geometry is corrupted when some of its vertices are incorrectly placed relative to the other vertices. They tend to happen during animations that depend on physics with flexible/dynamic geometries, but can also happen with static geometry (although this is less common).
=== "Implementation"
    Randomly modifies the vertex positions of a chosen game object geometry over time.  

<iframe width="560" height="315" src="https://www.youtube.com/embed/MWep9tcnwzc?playlist=MWep9tcnwzc&rel=0&loop=1&modestbranding=1&autohide=1&mute=1&showinfo=0" title="YouTube video player" frameborder="0" allow="accelerometer; autoplay; clipboard-write; encrypted-media; gyroscope; picture-in-picture" allowfullscreen style="border:none;overflow:hidden;display:block;margin:0 auto;"></iframe> 

-------------------------- 


## Screen Tear
=== "Description"
    Screen tearing happens when a monitors refresh rate doesn't match the GPU frame rate. This leads to information multiple frames being rendered to the screen as a single frame, leading to a tearing effect. Although typically a hardware problem, it represents an interesting and often subtle visual issue.
=== "Implementation"
    Record previous frames and render them embedded in the current frame.  

<iframe width="560" height="315" src="https://www.youtube.com/embed/laYDOshnd7k?playlist=laYDOshnd7k&rel=0&loop=1&modestbranding=1&autohide=1&mute=1&showinfo=0" title="YouTube video player" frameborder="0" allow="accelerometer; autoplay; clipboard-write; encrypted-media; gyroscope; picture-in-picture" allowfullscreen style="border:none;overflow:hidden;display:block;margin:0 auto;"></iframe> 

-------------------------- 

## Texture Corruption
=== "Description"
    A texture may render incorrectly/become corrupt for various reasons, for example, when texture offsets incorrectly set, the UV map is incorrect or as a result of shader related issues. Similar effects may also be due to lighting problems, for example if the vertex normals are incorrectly set.  

=== "Implementation"
    The UV map/offsets are modified to corrupt the texture. 

<iframe width="560" height="315" src="https://www.youtube.com/embed/HhBr9_5GLEY?playlist=HhBr9_5GLEY&rel=0&loop=1&modestbranding=1&autohide=1&mute=1&showinfo=0" title="YouTube video player" frameborder="0" allow="accelerometer; autoplay; clipboard-write; encrypted-media; gyroscope; picture-in-picture" allowfullscreen style="border:none;overflow:hidden;display:block;margin:0 auto;"></iframe> 

-------------------------- 

## Texture Missing
=== "Description"
    When a texture is missing from an object, which may happen if a developer forgets to set the texture, its missing/inaccessible in the file system or otherwise, the shader responsible for rendering the texture will typically resort to rendering the object it in one colour (typically bright pink) or resort to a default texture.
=== "Implementation"
    Remove the texture from a chosen game object.  

<iframe width="560" height="315" src="https://www.youtube.com/embed/Z1BB4bf1mC0?playlist=Z1BB4bf1mC0&rel=0&loop=1&modestbranding=1&autohide=1&mute=1&showinfo=0" title="YouTube video player" frameborder="0" allow="accelerometer; autoplay; clipboard-write; encrypted-media; gyroscope; picture-in-picture" allowfullscreen style="border:none;overflow:hidden;display:block;margin:0 auto;"></iframe> 

-------------------------- 

## Z-Clipping
=== "Description"
    Rendering pipelines often have layers, layers contain geometry to be rendered and are queued and rendered in order. Pixels in the layers rendered subsequently are replaced or are blended with pixels in previous layers. If an object geometry is placed in the wrong layer, it may be rendered on top of others leading to some strange spatial effects (which may or may not be desirable).
=== "Implementation"
    Places a chosen game object in a layer that is always rendered last.  

<iframe width="560" height="315" src="https://www.youtube.com/embed/5kPJbN8A7m4?playlist=5kPJbN8A7m4&rel=0&loop=1&modestbranding=1&autohide=1&mute=1&showinfo=0" title="YouTube video player" frameborder="0" allow="accelerometer; autoplay; clipboard-write; encrypted-media; gyroscope; picture-in-picture" allowfullscreen style="border:none;overflow:hidden;display:block;margin:0 auto;"></iframe> 

-------------------------- 

## Z-Fighting
=== "Description"
    Z-Fighting happens when two surfaces have the same depth (z). The renderer/shader does not know which to show first and this results in a mixing of textures from the two surfaces. A flickering effect may also occur during when the players view shifts.
=== "Implementation"
    Copies a game object, modifies its texture and places it exactly at the position of the original. 


<iframe width="560" height="315" src="https://www.youtube.com/embed/l6c3GmiNqYU?playlist=l6c3GmiNqYU&rel=0&loop=1&modestbranding=1&autohide=1&mute=1&showinfo=0" title="YouTube video player" frameborder="0" allow="accelerometer; autoplay; clipboard-write; encrypted-media; gyroscope; picture-in-picture" allowfullscreen style="border:none;overflow:hidden;display:block;margin:0 auto;"></iframe> 


-------------------------- 


## Out of Bounds
!!! todo
    This feature is available but undocumented.

=== "Description"
    When a player can escape the playable area, accessing areas they shouldn't be able to access.
=== "Implementation"
    Removing/adding an object that otherwise prevents/enables the player from going out of bounds or otherwise provide some means for the player to escape the usual playable area. It might otherwise be caused by a [High Force](#high-force) bug.

-------------------------- 


## Invisible Wall
!!! todo
    This feature is in development

=== "Description"
    A wall that the player cannot see but cannot move through. Sometimes these are added to stop the player going out of bounds, but this can break immersion. It may also happen unintentionally if some geometry is not correctly rendered or as part of a broken game mechanic.
=== "Implementation"
    Add an invisible object in the scene somewhere it isn't supposed to be.


-------------------------- 

## Freezing
!!! todo
    This feature is in development

=== "Description"

    Everything freezes for some period of time. Sometimes the game may recover and "skip time" or resume normally. 
   
=== "Implementation"

    If skip time, a frame can be re-rendered to the screen for the period, otherwise all relevant scripts need to be disabled.

-------------------------- 

## Unresponsive

!!! todo
    This feature is in development

=== "Description"
    The player can no longer take any action. Similar to a [Freezing](#freezing) bug, however in this case only the player character is stuck.
   
=== "Implementation"
    Disable the player controller, each action becomes 'none'. 

-------------------------- 

## High Force

!!! todo
    This feature is in development

=== "Description"

    An object in the scene suddenly has a large force applied to it causing it shoot across the screen. This can also happen to he player character cause it to fly into the sky or off in some random direction. This most commonly happens when an dynamic (physics based) object gets trapped between two non-physics based objects, or if the time delta in the physics engine is to large. 
   
=== "Implementation"

    Squash an object between too static objects, or randomly apply a large force to the object/player.

=== "Example"
    <figure markdown>
    ![](../imgs/skyrim-high-force.jpg){ width="300" lazyload=true}
    <figcaption>Example of high force bug in the game Skyrim.</figcaption>
    </figure>

-------------------------- 

## Floating Objects

!!! todo
    This feature is in development

=== "Description"

    A
   
=== "Implementation"

-------------------------- 









