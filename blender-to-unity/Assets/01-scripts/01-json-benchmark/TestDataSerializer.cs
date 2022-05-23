using UnityEngine;
using UnityEditor;
using Sirenix.OdinInspector;
using System;
using Newtonsoft.Json;
using System.Diagnostics;

namespace JsonBenchmark
{
    [ExecuteInEditMode]
    public class TestDataSerializer : MonoBehaviour
    {
        public enum JSONSerializer {UNITY, JSON_NET, JSON_NET_OBJECT, JSON_NET_DICT}
        [SerializeField]
        private JSONSerializer jsonSerializer;

        public TestData inputTestData = new TestData();
        [ReadOnly]
        public TestData outputTestData;

        private string inputjson = "";
        private string outputjson = "";

        [SerializeField]
        private Mesh mesh;

        [Button]
        private void MeshVertsToTestData(){
            inputTestData.testExplicit.positions = mesh.vertices;
        }

        [Button]
        private void SerialiseData(){

            var serializeData = Stopwatch.StartNew();
            inputjson = EditorJsonUtility.ToJson(inputTestData);
            serializeData.Stop();

            print($"{jsonSerializer} serialize time: {serializeData.ElapsedMilliseconds}");
        }
        [Button]
        private void WriteData()
        {
            var writeData = Stopwatch.StartNew();
            System.IO.File.WriteAllText(@"E:\repos\blender-to-unity\blender-to-unity\Assets\01-scripts\01-json-benchmark\_jsondata\inputjson.json", inputjson);
            writeData.Stop();
            print($"Write time: {writeData.ElapsedMilliseconds}");
        }
        [Button]
        private void ReadData()
        {
            var readData = Stopwatch.StartNew();
            outputjson = System.IO.File.ReadAllText(@"E:\repos\blender-to-unity\blender-to-unity\Assets\01-scripts\01-json-benchmark\_jsondata\inputjson.json");
            readData.Stop();
            print($"Read time: {readData.ElapsedMilliseconds}");
        }
        [Button]
        private void DeserialiseData()
        {
            var deserializeData = Stopwatch.StartNew();

            EditorJsonUtility.FromJsonOverwrite(outputjson, outputTestData);

            deserializeData.Stop();

            print($"{jsonSerializer} deserialize time: {deserializeData.ElapsedMilliseconds}");
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
        }

        private void TestOutputData()
        {
            UnityEngine.Debug.Log($"Test Data Child: {(Child)(outputTestData.parents[0])}");
        }

    }
}