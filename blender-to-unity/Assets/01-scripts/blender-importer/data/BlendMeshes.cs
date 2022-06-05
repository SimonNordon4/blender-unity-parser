using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Blend Meshes Data
/// </summary>

[Serializable]
/// <summary>
/// Top level class for blend meshes
/// </summary>
public class BlendMeshes
{
    public BlendMesh[] meshes;
}

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
