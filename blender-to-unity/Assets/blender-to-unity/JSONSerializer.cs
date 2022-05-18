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
        var tc = new TestChild();
        var tc2 = new TestChild();
        var tc3 = new TestChild();

        tc.name = "Test 1";
        tc.age = 3;
        tc.parent = tc2;

        tc2.name = "Test 2";
        tc2.age = 4;
        tc2.parent = null;

        tc3.name = "Test 3";
        tc3.age = 5;
        tc3.parent = tc2;

        var p = new TestParent();
        p.children = new TestChild[] { tc, tc2, tc3 };

        var data = JsonConvert.SerializeObject(p);
        Debug.Log(data);
        File.WriteAllText(unityPath + uJson, data);
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