#if UNITY_EDITOR

using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using System.IO;
using BlenderToUnity;
using System;
using System.Collections.Generic;

[ExecuteInEditMode]
public class JSONSerializer : MonoBehaviour
{
    [SerializeField] private Components uBlend;
    private static string projectPath = @"E:\repos\blender-to-unity\json-test\";
    private static string unityPath = @"E:\repos\blender-to-unity\blender-to-unity\Assets\blender-to-unity\";

    private static string uJson = "data_unity.json";
    private static string uBlendFile = "data_unity.ublend";

    [SerializeField]
    public Dictionary<string,object> testDict = new Dictionary<string, object>();

    public void SaveToFile(object data)
    {
        var json = JsonConvert.SerializeObject(data, Formatting.Indented);
        Debug.Log(json);
        File.WriteAllText(unityPath + uBlendFile, json);
        UnityEditor.AssetDatabase.Refresh();

        var resultJson = File.ReadAllText(unityPath + uBlendFile);

        //var result = JsonConvert.DeserializeObject<ObjectData>(resultJson);
        // data = result;

        // var resultRecast = JsonConvert.SerializeObject(result, Formatting.Indented);
        // Debug.Log(resultRecast);
    }

    private void OnEnable() {
        Test();
    }

    public void Test()
    {
        var x = new Components();
        x.components = new List<Component>(){new Component(), new Teleport(),new Collider()};
        var settings = new JsonSerializerSettings();
        settings.TypeNameHandling = TypeNameHandling.Objects;
        var json = JsonConvert.SerializeObject(x,settings);
        var result = JsonConvert.DeserializeObject<Components>(json,settings);

        Debug.Log($"Serialized: {json}");
        Debug.Log($"Deserialized: {JsonConvert.SerializeObject(result,settings)}");

        uBlend = result;
        
        //
    }
}

[System.Serializable]
public class Components{
    [SerializeReference]
    public List<Component> components = new List<Component>();
}

[System.Serializable]
public class Component
{
    public string type = "Component";
}
[System.Serializable]
public class Teleport : Component
{
    new public string type = "Teleport";
    public Vector3 location;
}

public class Collider : Component
{
    new public string type = "Collider";
    public int isTrigger;
}

// TODO: Replace the serialisation of $type: so that we can convert Python Class to C# Class easily and sexily. 
// https://stackoverflow.com/questions/13598969/json-net-deserializing-polymorphic-types-without-specifying-the-assembly
// https://www.newtonsoft.com/json/help/html/SerializeSerializationBinder.htm
// https://www.newtonsoft.com/json/help/html/serializationsettings.htm#TypeNameHandling
// https://thematrix.bw.edu/cmidkiff/FittsLaw/raw/commit/be25651b2135b3e51b6662bc0f23c2b64899f502/Assets/JsonDotNet/Documentation/Json%20Net%20for%20Unity%202.0.1.pdf
// public class KnownTypesBinder : ISerializationBinder
// {
//     public IList<Type> KnownTypes { get; set; }

//     public Type BindToType(string assemblyName, string typeName)
//     {
//         return KnownTypes.SingleOrDefault(t => t.Name == typeName);
//     }

//     public void BindToName(Type serializedType, out string assemblyName, out string typeName)
//     {
//         assemblyName = null;
//         typeName = serializedType.Name;
//     }
// }
#endif