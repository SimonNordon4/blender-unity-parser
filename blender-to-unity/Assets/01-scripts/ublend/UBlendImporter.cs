using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.AssetImporters;
using System.IO;
using Sirenix.OdinInspector;
using System.Diagnostics;

namespace UBlend
{
    [ScriptedImporter(1, "ublend")]
    public class UBlendImporter : ScriptedImporter
    {
        [ReadOnly]
        public UBlend m_uBlend;

        private Dictionary<string, Mesh> _meshIdMap = new Dictionary<string, Mesh>();

        public override void OnImportAsset(AssetImportContext ctx)
        {
            var readTime = Stopwatch.StartNew();
            var json = File.ReadAllText(ctx.assetPath);
            readTime.Stop();

            var deserializeTime = Stopwatch.StartNew();
            if (m_uBlend == null) m_uBlend = new UBlend();
            EditorJsonUtility.FromJsonOverwrite(json, m_uBlend);
            deserializeTime.Stop();

            UnityEngine.Debug.Log($"Read time: {readTime.ElapsedMilliseconds}ms");
            UnityEngine.Debug.Log($"Deserialize time: {deserializeTime.ElapsedMilliseconds}ms");

            var name = Path.GetFileNameWithoutExtension(ctx.assetPath);
            var go = new GameObject(name);
            ctx.AddObjectToAsset(name, go);
            ctx.SetMainObject(go);

            var meshTime = Stopwatch.StartNew();
            CreateMeshes(ctx, m_uBlend);
            meshTime.Stop();
            UnityEngine.Debug.Log($"Mesh time: {meshTime.ElapsedMilliseconds}ms");
        }

        public void CreateMeshes(AssetImportContext ctx, UBlend ublend)
        {
            foreach (var u_mesh in ublend.u_meshes)
            {
                Mesh mesh = new Mesh();
                mesh.name = u_mesh.name;
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

                for (int i = 0; i < u_mesh.submeshes.Length; i++)
                {
                    mesh.SetTriangles(u_mesh.submeshes[i].triangles, i);
                }

                _meshIdMap.Add(u_mesh.name, mesh);
                ctx.AddObjectToAsset(u_mesh.name, mesh);
            }
        }

        private void Log(object obj)
        {
            UnityEngine.Debug.Log(obj);
        }
    }
}