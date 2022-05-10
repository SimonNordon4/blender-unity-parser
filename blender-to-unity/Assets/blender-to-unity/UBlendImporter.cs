#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.AssetImporters;
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
        print($"Mesh Name: {uMesh.name}"); // Name
        for( int i = 0; i < uMesh.vertices.Count; i++) print($"Vertex[{i}]: {uMesh.vertices[i]}"); // Vertices
        for( int i = 0; i < uMesh.normals.Count; i++) print($"Normal[{i}]: {uMesh.normals[i]}"); // Normals
        for( int i = 0; i < uMesh.triangles.Count; i++) print($"Triangle[{i}]: {uMesh.triangles[i]}"); // Triangles

        // Create the object!
        var go = new GameObject(fileName);
        
        Mesh mesh = new Mesh();

        mesh.vertices = Vec3ToVector3(uMesh.vertices);
        mesh.normals = Vec3ToVector3(uMesh.normals);
        mesh.triangles = Vec3IntToInt(uMesh.triangles);

        var mf = go.AddComponent<MeshFilter>();
        mf.mesh = mesh;

        go.AddComponent<MeshRenderer>();
    }

    // TODO experiment with direct Mesh Serialisation.

    public Vector3[] Vec3ToVector3(List<Vec3> vecs)
    {
        Vector3[] vector3s = new Vector3[vecs.Count];

        for (int i = 0; i < vecs.Count; i++)
        {
            vector3s[i] = new Vector3(vecs[i].x,vecs[i].y,vecs[i].z);
        }

        return vector3s;
    }

    public int[] Vec3IntToInt(List<Vec3Int> vecs)
    {
        // Triangle is just an array of ints so we need to break out our vectors into ints next to eachother...
        var vecInts = new List<int>();

        foreach (var vec in vecs)
        {
            vecInts.Add(vec.x);
            vecInts.Add(vec.y);
            vecInts.Add(vec.z);
        }

        var tris = new int[vecInts.Count];
        for (int i = 0; i < vecInts.Count; i++)
        {
            tris[i] = vecInts[i];
        }
        return tris;
    }

    public void print(object obj)
    {
        Debug.Log(obj);
    }

    #region Utility Classes

    public class Vec3
    {
        public Vec3(float _x, float _y, float _z)
        {
            x = _x;
            y = _y;
            z = _z;
        }

        public float x;
        public float y;
        public float z;

        public override string ToString()
        {
            return $"({x:0.0000},{y:0.0000},{z:0.0000})";
        }
    }

    public class Vec3Int
    {
        public Vec3Int(int _x, int _y, int _z)
        {
            x = _x;
            y = _y;
            z = _z;
        }

        public int x;
        public int y;
        public int z;

        public override string ToString()
        {
            return $"({x},{y},{z})";
        }
    }

    #endregion

    #region Asset Classes

    public class UMesh
    {
        public string name = "";
        public List<Vec3> vertices = new List<Vec3>();
        public List<Vec3> normals = new List<Vec3>();
        public List<Vec3Int> triangles = new List<Vec3Int>();
    }

    #endregion
}


#endif