using System.Collections.Generic;
using UnityEngine;
using System;
using Newtonsoft.Json;
using System.Reflection;

namespace BlenderToUnity
{
    [Serializable]
    public class UBlendType
    {
        //public string id = "";
    }

    [Serializable]
    public class UBlendData : UBlendType
    {
        public List<UGameObject> uGameObjects = new List<UGameObject>();
        public List<UMesh> uMeshes = new List<UMesh>();

        public const string uGameObjectsKey = "u_gameobjects";
        public const string uMeshesKey = "u_meshes";
    }

    [Serializable]
    public class UMesh : UBlendType
    {
        
    }

    [Serializable]
    public class UGameObject : UBlendType
    {
        public string uName = "";
        [SerializeReference]
        public List<UComponent> uComponents = new List<UComponent>();

        public const string uNameKey = "u_name";
        public const string uComponentsKey = "u_components";
    }

    [Serializable]
    public class UComponent : UBlendType
    {
        //public Type type = null; We don't need to store the type, it just has to be present in the json.
        public const string uTypeKey = "u_type";
    }

    [Serializable]
    public class UTransform : UComponent
    {
        public string parentName = null;
        public Vector3 position = Vector3.zero;
        public Vector3 rotation = Vector3.zero;
        public Vector3 scale = Vector3.one;

        public const string parentNameKey = "u_parent";
        public const string positionKey = "u_position";
        public const string rotationKey = "u_rotation";
        public const string scaleKey = "u_scale";
    }

    [Serializable]
    public class UMeshFilter : UComponent
    {
        public string meshName = "";
        public const string meshNameKey = "u_mesh_ref";
    }

    // /// <summary>
    // /// The main Data Chunk of the UBlend JSON
    // /// </summary>
    // [System.Serializable]
    // public class UBlendData
    // {
    //     [JsonProperty("u_gameobjects")]
    //     public UGameObject[] uGameObjects;

    //     [JsonProperty("u_meshes")]
    //     public UMesh[] uMeshes;
    // }

    // [System.Serializable]
    // public class UGameObject
    // {
    //     public string name;
    //     public UTransform transform;
    //     public UComponent[] components;
    // }
    // [System.Serializable]
    // public class UTransform
    // {
    //     public string parent; // Strings are unique in Blender, so we can use bpy.object.name
    //     public Vector3 position;
    //     public Vector3 eularAngles;
    //     [JsonProperty("lossy_scale")]
    //     public Vector3 lossyScale;
    // }
    // [System.Serializable]
    // public class UComponent
    // {
    //     public string u_type;
    // }
    // [System.Serializable]
    // public class UMeshFilter
    // {
    //     public string mesh;
    // }

    // [System.Serializable]
    // public class UMesh
    // {
    //     public string name = "";
    //     public Vector3[] vertices = new Vector3[0];
    //     public Vector3[] normals = new Vector3[0];
    //     [JsonProperty("submesh_triangles")]
    //     public int[][] subMeshTriangles = new int[0][]; // mesh.tirangles doesn't support multiple submeshes, so instead all triangles should be considered belonging to a submesh, we use Mesh.SetTriangles
    //     [JsonProperty("submesh_count")]
    //     public int subMeshCount = 1;
    //     //TODO: add Vector4 uvs for additional mappings types? 
    //     public Vector2[][] uvs = new Vector2[0][];
    // }
}