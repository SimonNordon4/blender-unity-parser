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
        uMesh.triangles = meshToSerialize.triangles;

        Debug.Log($"Normals: {meshToSerialize.normals.Length}");
        
        var data = JsonConvert.SerializeObject(uMesh);
        File.WriteAllText(unityPath + uJson, data);
    }
}


#endif