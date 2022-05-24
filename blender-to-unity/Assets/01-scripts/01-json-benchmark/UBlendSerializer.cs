using UnityEngine;
using UnityEditor;
using Sirenix.OdinInspector;
using System;
using System.Diagnostics;

namespace UBlend
{
    [ExecuteInEditMode]
    public class UBlendSerializer : MonoBehaviour
    {
        public UBlend input_ublend;
        [ReadOnly]
        public UBlend output_ublend;
        private string inputjson = "";
        private string outputjson = "";

        private string jsonFileName = "mesh";



        private void SerialiseData(){

            var serializeData = Stopwatch.StartNew();
            inputjson = EditorJsonUtility.ToJson(input_ublend);
            serializeData.Stop();

            print($"serialize time: {serializeData.ElapsedMilliseconds}");
        }

        private void WriteData()
        {
            var writeData = Stopwatch.StartNew();
            
            System.IO.File.WriteAllText(@$"E:\repos\blender-to-unity\blender-to-unity\Assets\01-scripts\01-json-benchmark\_jsondata\ublend_new.json", inputjson);
            UnityEngine.Debug.Log(@$"E:\repos\blender-to-unity\blender-to-unity\Assets\01-scripts\01-json-benchmark\_jsondata\ublend_new.json");
            writeData.Stop();
            print($"Write time: {writeData.ElapsedMilliseconds}");
        }

        private void ReadData()
        {
            var readData = Stopwatch.StartNew();
            outputjson = System.IO.File.ReadAllText(@$"E:\repos\blender-to-unity\blender-to-unity\Assets\01-scripts\01-json-benchmark\_jsondata\ublend_new.json");
            readData.Stop();
            print($"Read time: {readData.ElapsedMilliseconds}");
        }

        private void DeserialiseData()
        {
            var deserializeData = Stopwatch.StartNew();
            EditorJsonUtility.FromJsonOverwrite(outputjson, output_ublend);

            deserializeData.Stop();

            print($"deserialize time: {deserializeData.ElapsedMilliseconds}");
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
            input_ublend = output_ublend;
            SerialiseData();
            WriteData();
            ReadData();
            DeserialiseData();
            AssetDatabase.Refresh();
        }

    }
}