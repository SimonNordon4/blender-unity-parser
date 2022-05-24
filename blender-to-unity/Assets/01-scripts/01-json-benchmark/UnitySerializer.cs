using UnityEngine;
using UnityEditor;
using Sirenix.OdinInspector;
using System;
using System.Diagnostics;

namespace UBlend
{
    [ExecuteInEditMode]
    public class UnitySerializer : MonoBehaviour
    {
        public UnityEngine.Object input_unity;
        [ReadOnly]
        public UnityEngine.Object output_unity;
        private string inputjson = "";
        private string outputjson = "";

        private string fileName = "object";




        private void SerialiseData(){

            var serializeData = Stopwatch.StartNew();
            inputjson = EditorJsonUtility.ToJson(input_unity);
            serializeData.Stop();

            print($"serialize time: {serializeData.ElapsedMilliseconds}");
        }

        private void WriteData()
        {
            var writeData = Stopwatch.StartNew();
            
            System.IO.File.WriteAllText(@$"E:\repos\blender-to-unity\blender-to-unity\Assets\01-scripts\01-json-benchmark\_jsondata\" + fileName+".json", inputjson);
            UnityEngine.Debug.Log(@$"E:\repos\blender-to-unity\blender-to-unity\Assets\01-scripts\01-json-benchmark\_jsondata\" + fileName +".json");
            writeData.Stop();
            print($"Write time: {writeData.ElapsedMilliseconds}");
        }

        private void ReadData()
        {
            var readData = Stopwatch.StartNew();
            outputjson = System.IO.File.ReadAllText(@$"E:\repos\blender-to-unity\blender-to-unity\Assets\01-scripts\01-json-benchmark\_jsondata\unity_new.json");
            readData.Stop();
            print($"Read time: {readData.ElapsedMilliseconds}");
        }

        private void DeserialiseData()
        {
            var deserializeData = Stopwatch.StartNew();
            EditorJsonUtility.FromJsonOverwrite(outputjson, output_unity);

            deserializeData.Stop();

            print($"deserialize time: {deserializeData.ElapsedMilliseconds}");
        }

        [Button(ButtonSizes.Large)]
        private void Serialize()
        {
            fileName = (input_unity.GetType().Name);
            SerialiseData();
            WriteData();
            AssetDatabase.Refresh();
        }

        [Button(ButtonSizes.Large)]
        private void SerializeDeserialize()
        {
            SerialiseData();
            WriteData();
            ReadData();
            DeserialiseData();
            AssetDatabase.Refresh();
        }

        [Button(ButtonSizes.Large)]
        private void SerializeDeserializeAgain()
        {
            input_unity = output_unity;
            SerialiseData();
            WriteData();
            ReadData();
            DeserialiseData();
            AssetDatabase.Refresh();
        }
    }

    [Serializable]
    public class UnityObject{
        [SerializeReference]
        public GameObject gameObject;
        public Transform transform;
        public Component component;
    }
}