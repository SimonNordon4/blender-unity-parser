using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.AssetImporters;
using System;
using System.IO;
using Sirenix.OdinInspector;
using System.Diagnostics;

namespace UBlend
{
    [ScriptedImporter(1, "ublend")]
    public class UBlendImporter : ScriptedImporter
    {
        public Material debugMaterial;

        [ReadOnly]
        public UBlend m_uBlend;

        private Dictionary<string, Mesh> _meshIdMap = new Dictionary<string, Mesh>();
        private Dictionary<string, Material> _materialIdMap = new Dictionary<string, Material>();
        private Dictionary<string, GameObject> _gameobjectIdMap = new Dictionary<string, GameObject>();

        

        public override void OnImportAsset(AssetImportContext ctx)
        {
            var readTime = Stopwatch.StartNew();
            var json = File.ReadAllText(ctx.assetPath);
            readTime.Stop();

            var parseTime = Stopwatch.StartNew();
            if (m_uBlend == null) m_uBlend = new UBlend();
            parseTime.Stop();
            var deserializeTime = Stopwatch.StartNew();
            EditorJsonUtility.FromJsonOverwrite(json, m_uBlend);
            deserializeTime.Stop();
            

            UnityEngine.Debug.Log($"Read time: {readTime.ElapsedMilliseconds}ms");
            UnityEngine.Debug.Log($"Parse time: {parseTime.ElapsedMilliseconds}ms");
            UnityEngine.Debug.Log($"Deserialize time: {deserializeTime.ElapsedMilliseconds}ms");

            var name = Path.GetFileNameWithoutExtension(ctx.assetPath);
            var rootGameObject = new GameObject(name);
            ctx.AddObjectToAsset(name, rootGameObject);
            ctx.SetMainObject(rootGameObject);


            CreateMeshes(ctx, m_uBlend);
            CreateMaterials(ctx, m_uBlend);
            CreateGameObjects(ctx, m_uBlend);
            CreateHierachy(m_uBlend,rootGameObject.transform);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();


        }

        public void CreateMeshes(AssetImportContext ctx, UBlend ublend)
        {
            var meshTime = Stopwatch.StartNew();

            foreach (var u_mesh in ublend.u_meshes)
            {
                Mesh mesh = new Mesh();
                mesh.name = u_mesh.name;
                //TODO: If more than 65k verts we need to int32
                mesh.SetVertices(u_mesh.vertices);
                mesh.SetNormals(u_mesh.normals);

                //Add all possible uvs
                if (u_mesh.uv.Length > 0)
                    mesh.SetUVs(0, u_mesh.uv);
                if (u_mesh.uv2.Length > 0)
                    mesh.SetUVs(1, u_mesh.uv2);
                if (u_mesh.uv3.Length > 0)
                    mesh.SetUVs(2, u_mesh.uv3);
                if (u_mesh.uv4.Length > 0)
                    mesh.SetUVs(3, u_mesh.uv4);
                if (u_mesh.uv5.Length > 0)
                    mesh.SetUVs(4, u_mesh.uv5);
                if (u_mesh.uv6.Length > 0)
                    mesh.SetUVs(5, u_mesh.uv6);
                if (u_mesh.uv7.Length > 0)
                    mesh.SetUVs(6, u_mesh.uv7);
                if (u_mesh.uv8.Length > 0)
                    mesh.SetUVs(7, u_mesh.uv8);

                mesh.subMeshCount = u_mesh.submeshes.Length;
                for (int i = 0; i <  mesh.subMeshCount; i++)
                {
                    mesh.SetTriangles(u_mesh.submeshes[i].triangles, i);
                }
                _meshIdMap.Add(u_mesh.name, mesh);
                ctx.AddObjectToAsset(u_mesh.name, mesh);
            }

            meshTime.Stop();
            UnityEngine.Debug.Log($"    Mesh created in: {meshTime.ElapsedMilliseconds}ms");
        }

        public void CreateMaterials(AssetImportContext ctx, UBlend ublend)
        {   
            var materialTime = Stopwatch.StartNew();

            foreach(UMaterial u_mat in ublend.u_materials)
            {
                Material material = null;
                Log($"SHADER TYPE {Enum.Parse<ShaderType>(u_mat.shader)}");
                if(Enum.Parse<ShaderType>(u_mat.shader) == ShaderType.LIT)
                {
                    material = new Material(Shader.Find("Universal Render Pipeline/Lit"));
                    material.name = u_mat.name;
                    material.SetFloat("_Surface", (float)Enum.Parse<RenderType>(u_mat.rendertype));
                    material.SetColor("_BaseColor", u_mat.base_color);
                    material.SetFloat("_Smoothness", (1.0f - u_mat.roughness));
                    material.SetFloat("_Metallic", u_mat.metallic);
                    material.SetColor("_EmissionColor", u_mat.emission_color);
                }

                if (material == null) return;
                
                _materialIdMap.Add(u_mat.name, material);
                ctx.AddObjectToAsset(u_mat.name, material);
            }

            materialTime.Stop();
            UnityEngine.Debug.Log($"    Materials created in: {materialTime.ElapsedMilliseconds}ms");
        }

        public void CreateGameObjects(AssetImportContext ctx, UBlend uBlend)
        {
            var gameObjectTime = Stopwatch.StartNew();

            foreach(UGameObject u_go in uBlend.u_gameobjects)
            {
                GameObject go = new GameObject(u_go.name);
                go.transform.position = u_go.position;
                go.transform.rotation = Quaternion.Euler(u_go.rotation * Mathf.Rad2Deg);
                go.transform.localScale = u_go.scale;
                var mf = go.AddComponent<MeshFilter>();
                mf.sharedMesh = _meshIdMap[u_go.mesh_name];
                var mr = go.AddComponent<MeshRenderer>();

                Material[] goMats = new Material[u_go.material_names.Length];
                for (int i = 0; i < goMats.Length; i++)
                {   
                    var mat = _materialIdMap[u_go.material_names[i]];
                    goMats[i] = mat;
                }
                
                mr.sharedMaterials = goMats;

                _gameobjectIdMap.Add(u_go.name, go);
                ctx.AddObjectToAsset(u_go.name, go);
            }

            gameObjectTime.Stop();
            UnityEngine.Debug.Log($"    GameObject created in: {gameObjectTime.ElapsedMilliseconds}ms");
            
        }

        public void CreateHierachy(UBlend uBlend,Transform root)
        {
            var hierarchyTime = Stopwatch.StartNew();

            foreach(UGameObject u_go in uBlend.u_gameobjects)
            {
                if(u_go.parent_name == string.Empty || u_go.parent_name == null)
                    _gameobjectIdMap[u_go.name].transform.SetParent(root);
                else
                    _gameobjectIdMap[u_go.name].transform.SetParent(_gameobjectIdMap[u_go.parent_name].transform);
            }

            hierarchyTime.Stop();
            UnityEngine.Debug.Log($"    Hierarchy created in: {hierarchyTime.ElapsedMilliseconds}ms");
        }

        private void Log(object obj)
        {
            UnityEngine.Debug.Log(obj);
        }
    }
}