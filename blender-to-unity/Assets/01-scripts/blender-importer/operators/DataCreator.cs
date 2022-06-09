using System.Collections;
using System.Collections.Generic;
using UnityEditor.AssetImporters;
using UnityEngine;
using System.IO;

namespace Blender.Importer
{
    public class DataCreator
    {
        public Dictionary<string,Mesh> meshMap = new Dictionary<string, Mesh>();
        
        public Dictionary<string,Material> matMap = new Dictionary<string, Material>();

        public Dictionary<string,GameObject> goMap = new Dictionary<string, GameObject>();

        public void CreateData(AssetImportContext ctx, BlendData blendData)
        {
            var rootObject = new GameObject(Path.GetFileNameWithoutExtension(ctx.assetPath));
            ctx.AddObjectToAsset(rootObject.name, rootObject);
            ctx.SetMainObject(rootObject);

            // Meshes
            foreach(BlendMesh bMesh in blendData.blend_meshes)
            {
                Mesh mesh = CreateMesh(bMesh);
                meshMap.Add(bMesh.name_id, mesh);
                ctx.AddObjectToAsset(mesh.name, mesh);
            }

            // Gameobjects
            foreach(BlendGameObject bGameObject in blendData.blend_gameobjects)
            {
                GameObject gameObject = CreateGameObject(bGameObject);
                goMap.Add(bGameObject.name_id, gameObject);
            }

            foreach(BlendGameObject bGameObject in blendData.blend_gameobjects)
            {
                if(bGameObject.parent_id == string.Empty)
                {
                    goMap[bGameObject.name_id].transform.parent = rootObject.transform;
                }
                else
                {
                    goMap[bGameObject.name_id].transform.parent = goMap[bGameObject.parent_id].transform;
                }
                ctx.AddObjectToAsset(bGameObject.name_id, goMap[bGameObject.name_id]);
            }
        }
        public Mesh CreateMesh(BlendMesh blendMesh)
        {
            var mesh = new Mesh();
            mesh.name = blendMesh.name_id;
            mesh.SetVertices(FloatToVector3(blendMesh.vertices));
            mesh.SetNormals(FloatToVector3(blendMesh.normals));
            mesh.subMeshCount = blendMesh.sub_meshes.Length;
            for (int i = 0; i < blendMesh.sub_meshes.Length; i++)
            {
                mesh.SetTriangles(blendMesh.sub_meshes[i].triangles, i);
            }
            return mesh;
        }
        public GameObject CreateGameObject(BlendGameObject blendGo)
        {
            var go = new GameObject(blendGo.name_id);
            go.transform.position = blendGo.position;
            go.transform.rotation = Quaternion.Euler(blendGo.rotation * Mathf.Rad2Deg);
            go.transform.localScale = blendGo.scale;

            // Materials
            var slotNumbers = blendGo.material_slots > 0 ? blendGo.material_slots : 1; // consider no slots
            Material[] mats = new Material[slotNumbers];
            for (int i = 0; i < slotNumbers; i++)
            {
                mats[i] = new Material(Shader.Find("Universal Render Pipeline/Lit"));

                var colors = new Color[8]{Color.gray, Color.red, Color.green, Color.blue, Color.cyan, Color.magenta, Color.yellow, Color.clear};

                var colorIndex = i - Mathf.FloorToInt(i / 8);
                // rand color for fun
                mats[i].color = colors[colorIndex];
            }

            // Mesh
            if(meshMap.ContainsKey(blendGo.mesh_id))
            {
                var meshFilter = go.AddComponent<MeshFilter>();
                meshFilter.mesh = meshMap[blendGo.mesh_id];
                
                var mr = go.AddComponent<MeshRenderer>();
                mr.sharedMaterials = mats;
            }

            return go;
        }
        private static Vector3[] FloatToVector3(float[] floats)
        {
            var vectors = new Vector3[floats.Length / 3];
            for (int i = 0; i < vectors.Length; i++)
            {
                vectors[i] = new Vector3(floats[i * 3], floats[i * 3 + 1], floats[i * 3 + 2]);
            }
            return vectors;
        }
        private static Vector2[] FloatToVector2(float[] floats)
        {
            var vectors = new Vector2[floats.Length / 2];
            for (int i = 0; i < vectors.Length; i++)
            {
                vectors[i] = new Vector2(floats[i * 2], floats[i * 2 + 1]);
            }
            return vectors;
        }
    }
}