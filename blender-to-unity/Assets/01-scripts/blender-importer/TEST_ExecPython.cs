using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Sirenix.OdinInspector;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;

public class TEST_ExecPython : MonoBehaviour
{
    [ReadOnly]
    public string blendExecutable = @"C:\Program Files\Blender Foundation\Blender 3.1\blender.exe";
    [ReadOnly]
    public string pyPrint = @"E:\repos\blender-to-unity\blender-to-unity\Assets\01-scripts\blender-importer\TEST_py_print.py";

    [ReadOnly]
    public string pyWrite = @"E:\repos\blender-to-unity\blender-to-unity\Assets\01-scripts\blender-importer\TEST_py_write.py";

    [ReadOnly]
    public string jsonOutput = @"E:\repos\blender-to-unity\blender-to-unity\Assets\01-scripts\blender-importer\TEST_json.json";

    [ReadOnly]
    public string jsonPyOutput = @"E:\repos\blender-to-unity\blender-to-unity\Assets\01-scripts\blender-importer\TEST_json_export.json";

    private Stopwatch readWatch = new Stopwatch();

    [Button("Read From Output")]
    void StandardOutputTest()
    {
        var outputTest = new Stopwatch();
        outputTest.Start();
        ProcessStartInfo start = new ProcessStartInfo();
        start.FileName = blendExecutable;
        start.Arguments = $"--background --python {pyPrint}";
        start.UseShellExecute = false;
        start.RedirectStandardOutput = true;
        start.CreateNoWindow = true;

        using (Process process = Process.Start(start))
        {
            using (StreamReader reader = process.StandardOutput)
            {
                string result = reader.ReadToEnd();
                UnityEngine.Debug.Log(result);
            }
        }
        outputTest.Stop();
        UnityEngine.Debug.Log($"Output Test: {outputTest.ElapsedMilliseconds}ms");
    }

    [Button("Read From File")]
    void FileTest()
    {
        readWatch.Start();

        ProcessStartInfo start = new ProcessStartInfo();
        start.FileName = blendExecutable;
        start.Arguments = $"--background --python {pyWrite}";
        start.UseShellExecute = false;
        start.RedirectStandardOutput = true;
        start.CreateNoWindow = true;

       
        using (Process process = Process.Start(start))
        {
            using (StreamReader reader = process.StandardOutput)
            {
                string result = reader.ReadToEnd();
                UnityEngine.Debug.Log(result);
            }
        }
        

        // var output = File.ReadAllText(jsonPyOutput);
        // print(output);

        readWatch.Stop();
        print($"Read Time: {readWatch.ElapsedMilliseconds}ms");
    }

    [Button("Create Big JSON")]
    void BigJSONTest()
    {
        BigJson bj = new BigJson(1000);
        for(int i = 0; i < 1000; i++)
        {
            bj.vertices[i] = new Vector3(Random.Range(-100, 100), Random.Range(-100, 100), Random.Range(-100, 100));
        }

        var json = EditorJsonUtility.ToJson(bj);

        File.WriteAllText(jsonOutput, json);
    }
    void print(object obj){
    UnityEngine.Debug.Log(obj);
}
}
[System.Serializable]
public class BigJson
{
    public BigJson(int number){
        vertices = new Vector3[number];
    }
    public Vector3[] vertices;
}


