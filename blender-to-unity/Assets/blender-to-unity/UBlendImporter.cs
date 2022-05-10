#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.AssetImporters;
using UnityEditor;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

[ScriptedImporter(1, "ublend")]
public class UBlendImporter : ScriptedImporter
{
    public override void OnImportAsset(AssetImportContext ctx)
    {
        if(ctx is null)return;

        Debug.Log($"Importing {ctx.assetPath}");

        var fileName = Path.GetFileNameWithoutExtension(ctx.assetPath);
        var fileData = (File.ReadAllText(ctx.assetPath));
   
        // Verify Data
        Debug.Log(fileData);

        // Verify Result
        var uMesh = JsonConvert.DeserializeObject<UMesh>(fileData);


        // Create the object!
        CreateGameObject(uMesh,ctx.assetPath);
        
    }

    // TODO experiment with direct Mesh Serialisation

    public void CreateGameObject(UMesh uMesh, string filePath)
    {
        var go = new GameObject(Path.GetFileNameWithoutExtension(filePath));
        
        Mesh mesh = new Mesh();

        mesh.vertices = uMesh.vertices;
        mesh.normals = uMesh.normals;
        mesh.triangles = uMesh.triangles;

        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
        mesh.RecalculateTangents();

        var mf = go.AddComponent<MeshFilter>();
        mf.mesh = mesh;

        var mr = go.AddComponent<MeshRenderer>();
        mr.sharedMaterial = new Material(Shader.Find("Standard"));

        // Attempt to save.
        AssetDatabase.CreateAsset(mesh,Path.GetDirectoryName(filePath) + "mesh.asset");
        AssetDatabase.SaveAssets();
    }

    public void print(object obj)
    {
        Debug.Log(obj);
    }

    #region Utility Classes

    #endregion

    #region Asset Classes

    public class UMesh
    {
        public string name = "";
        public Vector3[] vertices = new Vector3[0];
        public Vector3[] normals = new Vector3[0];
        public int[] triangles = new int[0];
    }

    #endregion
}


#endif