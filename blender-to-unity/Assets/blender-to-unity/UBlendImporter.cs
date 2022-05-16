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
        // TODO use ctx.AddObjectToAsset() to create a proper prefab.
        if (ctx is null) return;

        Debug.Log($"Importing {ctx.assetPath}");

        var fileName = Path.GetFileNameWithoutExtension(ctx.assetPath);
        var filePath = Path.GetDirectoryName(ctx.assetPath);
        var fileData = (File.ReadAllText(ctx.assetPath));

        // Verify Data
        Debug.Log(fileData);

        // Verify Result - this will later deserialize the entire thing, not just a mesh.
        var uMesh = JsonConvert.DeserializeObject<UMesh>(fileData);

        CreateGameObject(uMesh,filePath);
    }

    // Top level prefab for the entire object.

    public Mesh CreateMesh(UMesh uMesh)
    {
        Mesh mesh = new Mesh();

        mesh.SetVertices(uMesh.vertices);
        mesh.SetNormals(uMesh.normals);

       

        return mesh;
    }


    public void CreateGameObject(UMesh uMesh, string filePath)
    {
        var go = new GameObject(Path.GetFileNameWithoutExtension(filePath));

        //todo if Mesh is greater than ~65,000K then mapMesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
        Mesh mesh = CreateMesh(uMesh);

        // todo make this a prefab.
        var mf = go.AddComponent<MeshFilter>();
        mf.mesh = mesh;

        // todo check material parameters.
        var mr = go.AddComponent<MeshRenderer>();
        mr.sharedMaterial = new Material(Shader.Find("Shader Graphs/uv"));

        // Attempt to save.
        AssetDatabase.DeleteAsset(@"Assets/blender-to-unity/mesh.asset");
        AssetDatabase.CreateAsset(mesh,@"Assets/blender-to-unity/mesh.asset");
        AssetDatabase.SaveAssets();
    }

    public void print(object obj)
    {
        Debug.Log(obj);
    }

    #region Utility Classes

    #endregion

    #region Asset Classes

    /// <summary>
    /// Unity Mesh Transport Container. https://docs.unity3d.com/ScriptReference/Mesh.html
    /// </summary>
    public class UMesh
    {
        public string name = "";
        public Vector3[] vertices = new Vector3[0];
        public Vector3[] normals = new Vector3[0];
        public int[][] subMeshTriangles = new int[0][]; // mesh.tirangles doesn't support multiple submeshes, so instead all triangles should be considered belonging to a submesh, we use Mesh.SetTriangles
        public int subMeshCount = 1;
        public Vector2[][] uvs = new Vector2[0][];
        // public Vector2[] uv = new Vector2[0];
        // public Vector2[] uv2 = new Vector2[0];
        // public Vector2[] uv3 = new Vector2[0];
        // public Vector2[] uv4 = new Vector2[0];
        // public Vector2[] uv5 = new Vector2[0];
        // public Vector2[] uv6 = new Vector2[0];
        // public Vector2[] uv7 = new Vector2[0];
    }

    #endregion
}


#endif