using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class BlendData
{
    public BlendMesh[] blend_meshes;

    public BlendGameObject[] blend_gameobjects;
}
/// <summary>
/// Blend Meshes Data
/// </summary>

[Serializable]
public class BlendMesh
{
    public string name_id = string.Empty;
    public float[] vertices = new float[0];
    public float[] normals = new float[0];

    public float[] uv = new float[0];
    public float[] uv2 = new float[0];
    public float[] uv3 = new float[0];
    public float[] uv4 = new float[0];
    public float[] uv5 = new float[0];
    public float[] uv6 = new float[0];
    public float[] uv7 = new float[0];
    public float[] uv8 = new float[0];
    public BlendSubMesh[] sub_meshes = new BlendSubMesh[0];
}

[Serializable]
public class BlendSubMesh
{
    public int[] triangles = new int[0];
}

[Serializable]
public class BlendGameObject
{
    public string name_id = string.Empty;

    // Transform
    public string parent_id = string.Empty;
    public Vector3 position = Vector3.zero;
    public Vector3 rotation = Vector3.zero;
    public Vector3 scale = Vector3.one;

    // Mesh
    public string mesh_id = string.Empty;
    public int material_slots = 1;
    public string[] material_ids = new string[0];
}