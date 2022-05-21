#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.AssetImporters;
using UnityEditor;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Sirenix.OdinInspector;


namespace UnityToBlender
{
    [ScriptedImporter(1, "ublend")]
    public class UBlendImporter : ScriptedImporter
    {
        [ReadOnly]
        public UBlendData uBlend;

        [ReadOnly]
        public JObjectViewer jobj; 

        public Dictionary<string,object> testDict;
        public override void OnImportAsset(AssetImportContext ctx)
        {
            // TODO use ctx.AddObjectToAsset() to create a proper prefab.
            if (ctx is null) return;

            Debug.Log($"Importing {ctx.assetPath}");

            var data = (File.ReadAllText(ctx.assetPath));

            // Incoming Data
            Debug.Log(data);

            var jLinq = JObject.Parse(data);

            jobj.test = JObjectViewer.JObjectToDictionary(jLinq);

            var uBlendData = JsonConvert.DeserializeObject<UBlendData>(data);
            uBlend = uBlendData;
            Util.ValidateImportData(uBlendData);

            var go = new GameObject("Test");
            var mat = new Material(Shader.Find("Standard"));
            ctx.AddObjectToAsset("my import", go);
            ctx.AddObjectToAsset("my material", mat);
            ctx.SetMainObject(go);
        }


        // public Mesh CreateMesh(UMesh uMesh)
        // {
        //     Mesh mesh = new Mesh();

        //     // CREATE VERTEX DATA
        //     mesh.SetVertices(uMesh.vertices);
        //     mesh.SetNormals(uMesh.normals);

        //     // CREATE TRIANGLES
        //     mesh.subMeshCount = uMesh.subMeshCount;
        //     for (int i = 0; i < uMesh.subMeshTriangles.Length; i++)
        //     {
        //         print($"Creating Submesh {i} of {uMesh.subMeshTriangles.Length}");
        //         mesh.SetTriangles(uMesh.subMeshTriangles[i], (i), true);
        //     }

        //     // CRATE UVS
        //     for (int i = 0; i < uMesh.uvs.Length; i++)
        //     {
        //         mesh.SetUVs(i, uMesh.uvs[i]);
        //     }
        //     return mesh;
        // }


        // public void CreateGameObject(UMesh uMesh, string assetPath)
        // {
        //     var go = new GameObject(Path.GetFileNameWithoutExtension(assetPath));

        //     //todo if Mesh is greater than ~65,000K then mapMesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
        //     Mesh mesh = CreateMesh(uMesh);

        //     // todo make this a prefab.
        //     var mf = go.AddComponent<MeshFilter>();
        //     mf.mesh = mesh;

        //     // todo check material parameters.
        //     var mr = go.AddComponent<MeshRenderer>();
        //     mr.sharedMaterial = new Material(Shader.Find("Shader Graphs/uv"));

        //     // Attempt to save.
        //     AssetDatabase.DeleteAsset(@"Assets/blender-to-unity/mesh.asset");
        //     AssetDatabase.CreateAsset(mesh, @"Assets/blender-to-unity/mesh.asset");
        //     AssetDatabase.SaveAssets();
        // }
    }

[System.Serializable]
    public class JObjectViewer
    {
        public Dictionary<string,object> test;

        public static Dictionary<string,object> JObjectToDictionary(JObject jObj)
        {
            var result = jObj.ToObject<Dictionary<string,object>>();
            return result;
        }
    }
}

public static class JObjectExtensions
{
    public static IDictionary<string, object> ToDictionary(this JObject @object)
    {
        var result = @object.ToObject<Dictionary<string, object>>();

        // var JObjectKeys = (from r in result
        //                    let key = r.Key
        //                    let value = r.Value
        //                    where value.GetType() == typeof(JObject)
        //                    select key).ToList();

        // var JArrayKeys = (from r in result
        //                   let key = r.Key
        //                   let value = r.Value
        //                   where value.GetType() == typeof(JArray)
        //                   select key).ToList();

        // JArrayKeys.ForEach(key => result[key] = ((JArray)result[key]).Values().Select(x => ((JValue)x).Value).ToArray());
        // JObjectKeys.ForEach(key => result[key] = ToDictionary(result[key] as JObject));

        return result;
    }
}
#endif

