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
        var uMesh = new UnityToBlender.UMesh();

        var uv1 = new Vector2[]{new Vector2(0,1),new Vector2(1,0)};
        var uv2 = new Vector2[]{new Vector2(0,1),new Vector2(1,0)};

        var uvs = new Vector2[][] {uv1,uv2};

        uMesh.uvs = uvs;

        var data = JsonConvert.SerializeObject(uMesh);
        Debug.Log(data);
        File.WriteAllText(unityPath + uJson, data);
    }
}

#endif