// #if UNITY_EDITOR

// using UnityEngine;
// using Sirenix.OdinInspector;
// using Newtonsoft.Json;
// using Newtonsoft.Json.Converters;
// using Newtonsoft.Json.Linq;
// using System.IO;
// using BlenderToUnity;
// using System.Collections.Generic;

// [ExecuteInEditMode]
// public class JSONSerializer : MonoBehaviour
// {
//     [SerializeField] private Mesh meshToSerialize;
//     //private static string projectPath = @"E:\repos\blender-to-unity\json-test\";
//     //private static string unityPath = @"E:\repos\blender-to-unity\blender-to-unity\Assets\blender-to-unity\";

//     private static string uJson = "data_unity.json";
//     private static string uBlend = "data_unity.ublend";

//     [SerializeField]
//     public Dictionary<string,object> testDict = new Dictionary<string, object>();

//     public void SaveToFile(object data)
//     {
//         var json = JsonConvert.SerializeObject(data, Formatting.Indented);
//         Debug.Log(json);
//         File.WriteAllText(unityPath + uBlend, json);
//         UnityEditor.AssetDatabase.Refresh();

//         var resultJson = File.ReadAllText(unityPath + uBlend);

//         var result = JsonConvert.DeserializeObject<ObjectData>(resultJson);
//         data = result;

//         var resultRecast = JsonConvert.SerializeObject(result, Formatting.Indented);
//         Debug.Log(resultRecast);
//     }

//     [Button]
//     public void ChildrenTest()
//     {

//         var go = new ObjectData();
//         //go.components = new Component[] {new Teleport(),new Collider()};
//         SaveToFile(go);
//     }

//     [Button]
//     public void DictionaryTest()
//     {
//         var go = new ObjectData();
//         //go.components = new Component[] {new Teleport(),new Collider()};
//         string json = JsonConvert.SerializeObject(go, Formatting.Indented);
//         JObject jss = JObject.Parse(json);
//         Debug.Log(jss["lists"][0]);
//     }

// }
// [System.Serializable]
// public class ObjectData
// {
//     public string name = "Test";
//     public int number = 3;
//     public int[] lists = new int[] { 1, 2, 3 };
//     //public Component[] components;
// }

// [System.Serializable]
// public class Component
// {
//     public string type;
// }
// [System.Serializable]
// public class Teleport : Component
// {
//     public Vector3 location;
// }

// public class Collider : Component
// {
//     public int isTrigger;
// }
// #endif