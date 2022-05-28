using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace UBlend
{
    public enum ShaderType {Standard = 0, Unlit = 1};
    public enum RenderType {Opaque = 0, Transparent = 1};


    [Serializable]
    public class UBlend
    {
        public UMesh[] u_meshes = new UMesh[0];
        public UMaterial[] u_materials = new UMaterial[0];
        public UGameObject[] u_gameobjects = new UGameObject[0];
    }

    #region Assets

    [Serializable]
    public class UMesh
    {
        public string name = string.Empty;
        // for serialisation purposes, make Vector3 a list of floats 3 times in length.
        public Vector3[] vertices = new Vector3[0];
        public Vector3[] normals = new Vector3[0];
        public Vector2[] uv = new Vector2[0];
        public Vector2[] uv2 = new Vector2[0];
        public Vector2[] uv3 = new Vector2[0];
        public Vector2[] uv4 = new Vector2[0];
        public Vector2[] uv5 = new Vector2[0];
        public Vector2[] uv6 = new Vector2[0];
        public Vector2[] uv7 = new Vector2[0];
        public Vector2[] uv8 = new Vector2[0];
        public USubMesh[] submeshes = new USubMesh[0];
    }

    [Serializable]
    public class USubMesh
    {
        public int[] triangles = new int[0];
    }

    /// <summary>
    /// packages -> URP
    /// </summary>
    [Serializable]
    public class UMaterial
    {
        public string name = string.Empty; 
        public string shader = ShaderType.Standard.ToString();
        public string rendertype = RenderType.Opaque.ToString();
        public Color base_color = Color.white;
        public float roughness = 1.0f;
        public float metallic = 0.0f;
        public Color emission_color = Color.black;
    }
    #endregion

    [Serializable]
    public class UGameObject
    {
        public string name = string.Empty;
        public int layer = 0;
        public string tag_string = string.Empty;
        public int static_editor_flags = 0;
        public bool is_active = true;

        // Transform
        public string parent_name = string.Empty;
        public Vector3 position = Vector3.zero;
        public Vector3 rotation = Vector3.zero;
        public Vector3 scale = Vector3.one;

        // Mesh
        public string mesh_name = string.Empty;
    }

    [Serializable]
    public class UComponents
    {
        public string u_gameobject_name = string.Empty;
    }
}