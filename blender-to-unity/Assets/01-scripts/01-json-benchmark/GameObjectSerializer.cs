using UnityEngine;
using UnityEditor;
using Sirenix.OdinInspector;
using System;
using Newtonsoft.Json;
using System.Diagnostics;
using BlenderToUnity;

namespace UBlend
{
    [ExecuteInEditMode]
    public class GameObjectSerializer : MonoBehaviour
    {
        // public GameObject input_go;
        // public GameObject output_go;

        // public Mesh input_mesh;
        // public Mesh output_mesh;

        public UBlend.Data input_ublend = new UBlend.Data();
        [ReadOnly]
        public UBlend.Data output_ublend = new UBlend.Data();
        private string inputjson = "";
        private string outputjson = "";

        private string jsonFileName = "mesh";


        [Button]
        private void SerialiseData(){

            var serializeData = Stopwatch.StartNew();
            inputjson = EditorJsonUtility.ToJson(input_ublend);
            serializeData.Stop();

            print($"serialize time: {serializeData.ElapsedMilliseconds}");
        }
        [Button]
        private void WriteData()
        {
            var writeData = Stopwatch.StartNew();
            
            System.IO.File.WriteAllText(@$"E:\repos\blender-to-unity\blender-to-unity\Assets\01-scripts\01-json-benchmark\_jsondata\ublend_new.json", inputjson);
            UnityEngine.Debug.Log(@$"E:\repos\blender-to-unity\blender-to-unity\Assets\01-scripts\01-json-benchmark\_jsondata\ublend_new.json");
            writeData.Stop();
            print($"Write time: {writeData.ElapsedMilliseconds}");
        }
        [Button]
        private void ReadData()
        {
            var readData = Stopwatch.StartNew();
            outputjson = System.IO.File.ReadAllText(@$"E:\repos\blender-to-unity\blender-to-unity\Assets\01-scripts\01-json-benchmark\_jsondata\{jsonFileName}.json");
            readData.Stop();
            print($"Read time: {readData.ElapsedMilliseconds}");
        }
        [Button]
        private void DeserialiseData()
        {
            var deserializeData = Stopwatch.StartNew();
            Data data = new Data();
            EditorJsonUtility.FromJsonOverwrite(outputjson, data);
            output_ublend = data;


            deserializeData.Stop();

            print($"deserialize time: {deserializeData.ElapsedMilliseconds}");
        }

        [Button(ButtonSizes.Large)]
        private void All()
        {
            SerialiseData();
            WriteData();
            ReadData();
            DeserialiseData();
            AssetDatabase.Refresh();
            TestOutputData();
            AssetDatabase.Refresh();
        }

        private void TestOutputData()
        {
            UnityEngine.Debug.Log(output_ublend.u_objects.u_gameobjects[0].name);
        }

    }
}