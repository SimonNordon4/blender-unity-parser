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
        public UBlendData input_ublend;
        [ReadOnly]
        public UBlendData output_ublend;
        private string inputjson = "";
        private string outputjson = "";

        public string uBlendFileName = "new";

        [ReadOnly]
        public string savePath = @$"E:\repos\blender-to-unity\blender-to-unity\Assets\01-scripts\01-json-benchmark\";

        [Button]
        public void SetPath()
        {
            savePath = EditorUtility.OpenFolderPanel("Select folder to save uBlend", savePath, "") + "\\";
        }

        private void SerialiseData(){

            var serializeData = Stopwatch.StartNew();
            inputjson = EditorJsonUtility.ToJson(input_ublend);
            serializeData.Stop();

            print($"serialize time: {serializeData.ElapsedMilliseconds}");
        }

        private void WriteData()
        {
            var writeData = Stopwatch.StartNew();
            
            System.IO.File.WriteAllText(savePath + uBlendFileName + ".ublend", inputjson);
            UnityEngine.Debug.Log(savePath + uBlendFileName + ".ublend");
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
        private void CreateUBlend()
        {
            SerialiseData();
            WriteData();
            AssetDatabase.Refresh();
        }
    }
}