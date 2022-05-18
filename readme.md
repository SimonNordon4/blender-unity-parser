# Blender to Unity

The goal is to create a Blender & Unity plugin that makes importing content from Blender to Unity seamless & easy. This will be achieved by exporting a .ublend _(UnityBlender)_ file format, a JSON file structured specifically for this task.

 _(Unity only allows custom importers on non reserved file types, hence creating a new one)_

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
 ## JSON Structure



```json
"Root" :
{
    "Scene" : {},
    "GameObjects" : {},
    "Assets" : {}
}
```
___
### Scene Data

Scene data is where we'll want to store data for the Unity Scene. Initially this will just be the name
of the root object and a Skybox Material Reference.

```json
"Scene" :
{
    "Name" : "SampleScene.Unity",
    "isImportSkybox" : false,
    "SkyBoxMaterial" : null,
    "renderPipeline" : "URP"
}
```
___
### GameObject Data
This will define a list of all gameobjects under the root.

```json
"GameObjects" :
[{
    "name" : "Cube001",
    "activeInHeirachy" : true,
    "isStatic" : false,
    "layer" : 0,
    "tag" : "Untagged",
    "scene" : "Scene.Unity", // This will be implied from the Scene Data
    "transform" : {},
    "components" : {},
}]
```

We define a custom transform Class

```json
"transform" :
{
    "parent" : null,
    "position" : {}, 
    "rotation" : {},
    "lossyScale" : {}
}
```
Finally we'll need a list of all components and their sub classes.

```json
"components" :
[{
    "MeshFilter" : {
        "Mesh" : {} // Asset Reference
    },
    "MeshRenderer":{
        "sharedMaterials" : [{}], // Asset Reference
        "recieveShadows" : false
        // etc
    },
    "BoxCollider":{
        "isTrigger" : false,
        "material": {},
        "center": {},
        "size" : {}
    }
}]
```
___
### Asset Data
Asset Data defines our usable assets. To begin this will include Mesh, Material & Texture. However can be expanded to AnimationClip, FontAsset, AudioClip etc

```json
"Assets" :
{
    "Meshes" : [{
        "name" : "Cube001",
        "vertices" : [{}],
        "normals" : [{}],
        "triangles" : [{}]
    }],
    "Textures" : [{
        "name" : "Untitled",
        "image" : "base64" // Should we embed to Texture Directly into the JSON?
    }],
    "Materials" : [{
        "name" : "Material",
        "color" : {},
        "mainTexture" : null,
        "shader" : {}
    }]
}
```

___

## Stratergies & Challenges

It's Blender Responsibility to ensure it produces a usable .ublend file. Unity simply reads the Data and maps it to it's relevant classes.

Scene:
        
    A Custom World Level Properties UILayout Panel for setting Scene Data. This can include Ambient Occlusion, Light Maps, Occlusion Data, etc.

Object:
        
    A Custom Object Level Properties UILayout Panel for setting GameObject Data (Including Components)

Mesh:
        
    Mesh Serializer with support for multi-material (sub-meshes)

Materials:

    Will Default to Standard or Lit for BDSF and similar. Unlit for Emission. We will then remap texture inputs to their Unity Equivilants.

Textures:

    Use Bake-to-Node to capture all input data as a texture. Will need global bake settings, and the ability to avoid rebaking if possible.

## To Do

[x] Import Mesh (.ublend)
[x] Import Gameobject (.ublend)
[x] Import Transform (.ublend)
[ ] Import MeshRenderer (.ublend)
[ ] Link Mesh to GameObject.MeshFilter