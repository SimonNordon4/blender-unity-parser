#if UNITY_EDITOR

using UnityEngine;
using Sirenix.OdinInspector;
using Newtonsoft.Json;
using System.IO;

[ExecuteInEditMode]
public class JSONSerializer : MonoBehaviour
{
    [SerializeField]private Mesh meshToSerialize;
    private static string projectPath = @"E:\repos\blender-to-unity\json-test\";
    private static string unityPath = @"E:\repos\blender-to-unity\blender-to-unity\Assets\blender-to-unity\";

    private static string uJson = "data_unity.json";
    [Button]
    public void SaveToFile()
    {
        var uMesh = new UBlendImporter.UMesh();
        uMesh.name = meshToSerialize.name;
        uMesh.vertices = meshToSerialize.vertices;
        uMesh.normals = meshToSerialize.normals;
        uMesh.subMeshCount = meshToSerialize.subMeshCount;
        uMesh.subMeshTriangles = new int[2][];
        uMesh.subMeshTriangles[0] = meshToSerialize.GetTriangles(0);
        uMesh.subMeshTriangles[1] = meshToSerialize.GetTriangles(1);

        Debug.Log($"Normals: {meshToSerialize.normals.Length}");

        Debug.Log($"Triangles: {meshToSerialize.triangles}");

        foreach(var tri in uMesh.subMeshTriangles)
        {
            Debug.Log(tri);
        }
        var data = JsonConvert.SerializeObject(meshToSerialize);
        File.WriteAllText(unityPath + uJson, data);
    }
}

#endif