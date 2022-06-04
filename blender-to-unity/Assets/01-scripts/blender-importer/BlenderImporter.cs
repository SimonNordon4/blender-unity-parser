using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.AssetImporters;
using System.Diagnostics;
using System.IO;

//This is Example Importer for cube
[ScriptedImporter(1, new[] { "cube1" }, new[] { "blend" })]
public class BlendImporter : ScriptedImporter
{
    private string pythonFile = @"E:\repos\blender-to-unity\blender-to-unity\Assets\01-scripts\blender-importer\python\get_blend_data.py";
    private string pythonExport = @"E:\\repos\\blender-to-unity\\blender-to-unity\\Assets\\01-scripts\\blender-importer\\blend_data.json";
    public override void OnImportAsset(AssetImportContext ctx)
    {
        print("This is completely new BLENDER importer!");
        ExecutePython(pythonFile);
        

        var cube = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        cube.transform.position = new Vector3(0, 0, 0);
        ctx.AddObjectToAsset("main obj", cube);
        ctx.SetMainObject(cube);
    }

    private string GetBlenderExecutablePath()
    {
        return @"C:\Program Files\Blender Foundation\Blender 3.1\blender.exe";
    }

    private void ExecutePython(string scriptPath)
    {
        var start = new ProcessStartInfo();
        start.FileName = GetBlenderExecutablePath();
        start.Arguments = $"--background --python {scriptPath}";
        start.UseShellExecute = false;
        start.RedirectStandardOutput = true;
        start.CreateNoWindow = true;

        using (Process process = Process.Start(start))
        {
            using (StreamReader reader = process.StandardOutput)
            {
                string result = reader.ReadToEnd();
                process.WaitForExit();
                ReadBlenderOutput();
            }
        }
    }

    private void ReadBlenderOutput()
    {
        print($"Read from imported file {File.ReadAllText(pythonExport)}");
        File.Delete(pythonExport);
        AssetDatabase.Refresh();
    }

    void print(object obj){ UnityEngine.Debug.Log(obj); }
}
