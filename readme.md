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

1. First we have to overwrite Unity's default 
.blend importer. We achieve this with an AssetPostProcessor that
Uses AssetDatabase.SetImporterOveride<BlendImporter> during the OnpreprocessAsset call.

2. We then will locate the systems blender executable and run 'blender -b -p unity_to_blender.py', launching a headless blender that exports the assets.

3. After opening the .blend file, Blender will then send it's data back to unity via a JSON _(very fast with orjson.py)_

4. This JSON will be deserialized with EditorJsonUtility _(Unity's JSON utility is limited, but very fast, and supports Vector3, Vector2, Color etc)_

5. Finally, the JSON data will be used to rebuild the asset.

___

## Features

Phase 1 will support;

- Gameobjects
- Meshes
- Materials
- Textures 

