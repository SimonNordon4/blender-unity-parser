#if UNITY_EDITOR

using UnityEngine;
using Sirenix.OdinInspector;
using Newtonsoft.Json;
using System.IO;

[ExecuteInEditMode]
public class JSONSerializer : MonoBehaviour
{
    
    private static string projectPath = @"E:\repos\blender-to-unity\json-test\";
    private static string unityPath = @"E:\repos\blender-to-unity\blender-to-unity\Assets\blender-to-unity\";

    private static string uJson = "data_unity.json";
    [Button]
    public void SaveToFile()
    {
        var test = new Test();
        test.nested = new Nested();
        
        var data = JsonConvert.SerializeObject(test);
        File.WriteAllText(unityPath + uJson, data);
    }
}

[System.Serializable]
public class Test
{
    public Nested nested {get;set;}
}

public class Nested{
    public Vector3[] vec = new Vector3[]{Vector3.back,Vector3.down};
}
#endif