_WARNING: this codebase is extremely volatile right now.

# Blender to Unity

The goal is to create a Blender & Unity plugin that makes importing content from Blender to Unity seamless & easy.

 _(Unity only 

___
## Goals

 For clarity we need to specify what this tool does. It transforms **Blender Data into a Unity Framework.** It does not convert a **Blender Framework into Unity Data.**

 In other words, you would expect to export Unity Classes from Blender. You wouldn't expect to Import Blender Classes into Unity.

For Example:

You can export a GameObject from Blender to Unity, but Blender doesn't have GameObjects. We need to extrapolate gameobject data from the Blender File by mapping similar data where possible, and giving the user the ability to fill in the gaps.

```cs
    Public Class Example
    {
        public UBlendData uData;
        public GameObject go;

        private void OnImported()
        {
            uo = uData.uObjects[0];
            go.name = uo.name // Some Data will map 1:1 from Blender
            go.SetActive(uo.isVisibleInViewport) // Some Data will have to be interpreted as 'close enough'
            go.tag = uo.customTag // Finally, some data will have to be added manually via custom addons
        }
    }
```

This could potentially get ugly as data lies across multiple classes. As an example, let's quickly go through Blend-File Data and possible Unity Equivilents.

```cs
        go.name = uo.name
        go.transform.position = uo.location
        go.meshrenderer.sharedmaterial = uo.material_slots[0].material
```

In order to simplify things, we should disregard Blender Data. We only care about Unity Data. We will then disregard Blender Data that doesn't have a Unity Purpose.

Blend-File Data | Unity Data
---|---
filename|fileName
file_has_unsaved_changes|-
file_is_saved|-
use_auto-pack|-
version|blenderVersion
cameras|cameras
scenes|-
objects|gameObjects
materials|materials
lights|lights
libraries|-
screens|-
window_managers|-
images|textures
lattices|-
.
.
.
worlds|skyboxes
collections|gameObjects
grease_pencil|-

You get the idea. Each of these data types will need to be carefully considered and mapped to the correct Unity Context.
___

## Strategy

1. Overwrite Unity's default blend importer.

2. Locate blender executable and run 'blender -b -p unity_to_blender.py'.

3. Send .blend data as JSON.

4. Deserialize JSON into intermediate class.

5. Construct and Serialise Unity Assets from intermediate class.

FAQ's:
_(Step 2) why not read .blend binary directly?_
Blender data isn't usable, it needs to be transmuted into Unity compatible. Bpy already provided many useful methods for doing this.

_(Step 3) why JSON?_
Serialising python to C# in binary is not trivial. JSONUtility is extremely fast and supports Unity data types like Vector3. It's.much easier to get python to produce unity readable JSON.
JSON has support like orjson and brotli. 

_(Step 4) why an intermediate class?_
We could deserialize directly to Unity data. However this data often carries a lot of blob we don't need. It's actually faster to not serialize it at all.
__

## Features

Phase 1 will support;

- Transform
- Meshes
- Materials
- Textures 
- ~Gameobjects (name, enabled)
- Mesh Filter
- Mesh Renderer
- Box Collider
- Mesh Collider

It will also have the option to export collections as gameibjects and export procedural materials to textures.


