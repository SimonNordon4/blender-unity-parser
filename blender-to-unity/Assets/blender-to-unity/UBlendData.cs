using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

namespace UnityToBlender
{
    /// <summary>
    /// The main Data Chunk of the UBlend JSON
    /// </summary>
    public class UBlendData
    {
        [JsonProperty("u_gameobjects")]
        public UGameObject[] uGameObjects;

        [JsonProperty("u_meshes")]
        public UMesh[] uMeshes;
    }

    public class UGameObject
    {
        public string name;
        public UTransform transform;
    }

    public class UTransform
    {
        public string parent;
        public Vector3 position;
        public Vector3 eularAngles;
        [JsonProperty("lossy_scale")]
        public Vector3 lossyScale;
    }

    public class UMesh
    {
        public string name = "";
        public Vector3[] vertices = new Vector3[0];
        public Vector3[] normals = new Vector3[0];
        [JsonProperty("submesh_triangles")]
        public int[][] subMeshTriangles = new int[0][]; // mesh.tirangles doesn't support multiple submeshes, so instead all triangles should be considered belonging to a submesh, we use Mesh.SetTriangles
        [JsonProperty("submesh_count")]
        public int subMeshCount = 1;
        //TODO: add Vector4 uvs for additional mappings types? 
        public Vector2[][] uvs = new Vector2[0][];
    }
}