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
using System.Reflection;


namespace BlenderToUnity
{
    [ScriptedImporter(1, "ublend")]
    public class UBlendImporter : ScriptedImporter
    {
        [ReadOnly]public UBlendData uBlend;
        public override void OnImportAsset(AssetImportContext ctx)
        {
            string json = File.ReadAllText(ctx.assetPath);
            JObject jObject = JObject.Parse(json);
            
            uBlend = UBlendOperations.JObjectToUBlendData(jObject);
            uBlend.uGameObjects[0].uComponents.Add(new UTransform());
        }
    }
}

#endif

 // [ScriptedImporter(1, "ublend")]
    // public class UBlendImporter : ScriptedImporter
    // {
    //     [ReadOnly]
    //     public UBlendData uBlend;

    //     public override void OnImportAsset(AssetImportContext ctx)
    //     {
    //         // TODO use ctx.AddObjectToAsset() to create a proper prefab.
    //         if (ctx is null) return;

    //         Debug.Log($"Importing {ctx.assetPath}");

    //         var data = (File.ReadAllText(ctx.assetPath));

    //         // Incoming Data
    //         Debug.Log(data);

    //         var jLinq = JObject.Parse(data);

    //         jobj.test = JObjectViewer.JObjectToDictionary(jLinq);

    //         var uBlendData = JsonConvert.DeserializeObject<UBlendData>(data);
    //         uBlend = uBlendData;
    //         Util.ValidateImportData(uBlendData);

    //         var go = new GameObject("Test");
    //         var mat = new Material(Shader.Find("Standard"));
    //         ctx.AddObjectToAsset("my import", go);
    //         ctx.AddObjectToAsset("my material", mat);
    //         ctx.SetMainObject(go);
    //     }


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
    

// [System.Serializable]
//     public class JObjectViewer
//     {
//         public Dictionary<string,object> test;

//         public static Dictionary<string,object> JObjectToDictionary(JObject jObj)
//         {
//             var result = jObj.ToObject<Dictionary<string,object>>();
//             return result;
//         }
//     }
// }

// public class Example
// {
//     private JObject jsonObject;
//     public void CreateMesh()
//     {
//         var mesh1 = new Mesh();
//         mesh1.SetTriangles((int[])jsonObject["mesh"]["triangles"],(int)jsonObject["subMeshs"][0]);

//         // or

//         var mesh2 = new Mesh();
//         var uMesh = new UMesh();

//         uMesh.triangles = (int[])jsonObject["mesh"]["triangles"];
//         uMesh.subMeshes = (int[])jsonObject["subMeshs"];

//         mesh2.SetTriangles(uMesh.triangles, uMesh.subMeshes[0]);
//     }
// }