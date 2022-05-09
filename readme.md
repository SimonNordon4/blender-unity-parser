# Blender to Unity

The goal is to create a Blender & Unity plugin that makes importing content from Blender to Unity seamless & easy. This will be achieved by exporting a .ublend _(UnityBlender)_ file format, a JSON file structured specifically for this task.

 _(Unity only allows custom importers on non reserved file types, hence creating a new one)_

___
 ## JSON Structure

 The resulting file can be categorized into 3 areas.

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
        
    A Custom World Level Properties UILayout Panel for setting Scene Data.

Object:
        
    A Custom Object Level Properties UILayout Panel for setting GameObject Data (Including Components)

Mesh:
        
    Mesh Serializer with support for multi-material (sub-meshes)

Materials:

    Will Default to Standard or Lit for BDSF and similar. Unlit for Emission. We will then remap texture inputs to their Unity Equivilants.

Textures:

    Use Bake-to-Node to capture all input data as a texture. Will need global bake settings, and the ability to avoid rebaking if possible.

## To Do

[x] Serialize a single material mesh (.ublend)
[ ] Import a single material mesh into Unity (.ublend)