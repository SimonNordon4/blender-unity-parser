using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace UBlend
{
    [Serializable]
    public class UBlend
    {
        public UMesh[] u_meshes = new UMesh[0];
        public UGameObject[] u_gameobjects = new UGameObject[0];
    }

    #region Assets

    [Serializable]
    public class UMesh
    {
        public string name = string.Empty;
        public Vector3[] vertices = new Vector3[0];
        public Vector3[] normals = new Vector3[0];
        public Vector2[] uv = new Vector2[0];
        public Vector2[] uv2 = new Vector2[0];
        public Vector2[] uv3 = new Vector2[0];
        public Vector2[] uv4 = new Vector2[0];
        public Vector2[] uv5 = new Vector2[0];
        public Vector2[] uv6 = new Vector2[0];
        public Vector2[] uv7 = new Vector2[0];
        public USubMesh[] u_sub_meshes = new USubMesh[0];
    }

    [Serializable]
    public class USubMesh
    {
        public int[] triangles = new int[0];
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

        public UTransform u_transform = new UTransform();
    }

    [Serializable]
    public class UTransform
    {
        public string parent_name = string.Empty;
        public Vector3 position = Vector3.zero;
        public Quaternion rotation = Quaternion.identity;
        public Vector3 scale = Vector3.one;
    }

    [Serializable]
    public class UComponents
    {
        public string u_gameobject_name = string.Empty;
    }
}