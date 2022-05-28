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
        [ReadOnly]
        public string savePath = @$"E:\repos\blender-to-unity\blender-to-unity\Assets\01-scripts\01-json-benchmark\_jsondata\";

        [Button]
        public void SetSavePath(){
            savePath = EditorUtility.OpenFolderPanel("Select folder to save", "", "");
        }

        private void SerialiseData(){

            var serializeData = Stopwatch.StartNew();
            inputjson = EditorJsonUtility.ToJson(input_unity);
            serializeData.Stop();

            print($"serialize time: {serializeData.ElapsedMilliseconds}");
        }

        private void WriteData()
        {
            var writeData = Stopwatch.StartNew();
            
            System.IO.File.WriteAllText(savePath + fileName+".json", inputjson);
            UnityEngine.Debug.Log(savePath + fileName +".json");
            writeData.Stop();
            print($"Write time: {writeData.ElapsedMilliseconds}");
        }

        private void ReadData()
        {
            var readData = Stopwatch.StartNew();
            outputjson = System.IO.File.ReadAllText(savePath + "unity_new.json");
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
            fileName = $"{(input_unity.GetType().Name)}_{input_unity.name}";
            SerialiseData();
            WriteData();
            AssetDatabase.Refresh();
        }


        private void SerializeDeserialize()
        {
            SerialiseData();
            WriteData();
            ReadData();
            DeserialiseData();
            AssetDatabase.Refresh();
        }


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