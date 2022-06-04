using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.AssetImporters;
using System.Diagnostics;
using System.IO;
using Sirenix.OdinInspector;
using UBlend;

//This is Example Importer for cube
[ScriptedImporter(1, new[] { "cube1" }, new[] { "blend" })]
public class BlendImporter : ScriptedImporter
{
    [ReadOnly]
    public UBlend.UBlend m_blend = new UBlend.UBlend();
    private string pythonFile = @"E:\repos\blender-to-unity\blender-to-unity\Assets\01-scripts\blender-importer\python\get_blend_data.py";
    private string pythonExport = @"E:\\repos\\blender-to-unity\\blender-to-unity\\Assets\\01-scripts\\blender-importer\\blender_export.json";
    public override void OnImportAsset(AssetImportContext ctx)
    {
        // First write data from blend file.
        ExecutePython(ctx,pythonFile);

        var cube = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        cube.transform.position = new Vector3(0, 0, 0);
        ctx.AddObjectToAsset("main obj", cube);
        ctx.SetMainObject(cube);
    }

    private string GetBlenderExecutablePath()
    {
        return @"C:\Program Files\Blender Foundation\Blender 3.1\blender.exe";
    }

    private void ExecutePython(AssetImportContext ctx,string scriptPath)
    {
        var start = new ProcessStartInfo();
        start.FileName = GetBlenderExecutablePath();
        start.Arguments = $"--background {ctx.assetPath} --python {scriptPath}";
        start.UseShellExecute = false;
        start.RedirectStandardOutput = true;
        start.CreateNoWindow = true;

        using (Process process = Process.Start(start))
        {
            using (StreamReader reader = process.StandardOutput)
            {
                string result = reader.ReadToEnd();
                print(result);
                ParseResult(result);
                process.WaitForExit();
                //ReadBlenderOutput();
            }
        }
    }

    private void ReadBlenderOutput()
    {
        print($"Read from imported file {File.ReadAllText(pythonExport)}");
        EditorJsonUtility.FromJsonOverwrite(File.ReadAllText(pythonExport),m_blend);
        File.Delete(pythonExport);
        AssetDatabase.Refresh();
    }
    void ParseResult(string result)
    {
        var vert = new List<float>();
        var x = result.Split("VertStart")[1].Split("VertEnd")[0];
        var vecs = x.Split("\n");
        foreach (var v in vecs)
        {
            var vec = v.Split(" ");
            foreach (var vv in vec)
            {
                if (vv != "")
                {
                    print(vv);
                    //vert.Add(float.Parse(vv));
                }
            }
        }

        foreach (var v in vert)
        {
            print(v);
        }
    }

    void print(object obj){ UnityEngine.Debug.Log(obj); }
}
