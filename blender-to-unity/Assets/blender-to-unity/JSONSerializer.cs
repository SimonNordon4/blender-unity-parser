#if UNITY_EDITOR

using UnityEngine;
using Sirenix.OdinInspector;
using Newtonsoft.Json;
using System.IO;
using UnityToBlender;

[ExecuteInEditMode]
public class JSONSerializer : MonoBehaviour
{
    [SerializeField]private Mesh meshToSerialize;
    private static string projectPath = @"E:\repos\blender-to-unity\json-test\";
    private static string unityPath = @"E:\repos\blender-to-unity\blender-to-unity\Assets\blender-to-unity\";

    private static string uJson = "data_unity.json";
    private static string uBlend = "data_unity.ublend";
    [Button]
    public void SaveToFile()
    {
        UBlendData data = new UBlendData();

        UGameObject uGameObject = new UGameObject();
        uGameObject.name = "test";
       
        data.uGameObjects = new UGameObject[] { uGameObject };

        var json = JsonConvert.SerializeObject(data, Formatting.Indented);
        Debug.Log(json);
        File.WriteAllText(unityPath + uBlend, json);

        UnityEditor.AssetDatabase.Refresh();
    }

    [System.Serializable]
    public class TestParent{
       public TestChild[] children;
    }
[System.Serializable]
    public class TestChild{
        public string name;
        public int age;
        public TestChild parent;
    }
}

#endif