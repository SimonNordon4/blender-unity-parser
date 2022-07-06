using UnityEditor;
using UnityEngine;
using Sirenix.OdinInspector;
using BlenderToUnity;

[ExecuteInEditMode]
public class BlendReaderInput : MonoBehaviour
{
    
    private string blendFilePath = @"E:\repos\blender-to-unity\blender-to-unity\Assets\01-scripts\blend-parser\BlenderToUnity\example.blend";
    private string saveFilePath = @"E:\repos\blender-to-unity\blender-to-unity\Assets\01-scripts\blend-parser\BlenderToUnity\example.json";

    private BlenderFileReader.BlenderFile oldBlenderFile;

    [ReadOnly]
    public BlenderFile blenderFile;

    [Button]
    private void ReadBlend()
    {
        blenderFile = new BlenderFile(blendFilePath);
    }

    [Button]
    private void SaveBlend()
    {
        f.startwatch("Json Serialize");
        var json = EditorJsonUtility.ToJson(blenderFile, true);
        System.IO.File.WriteAllText(saveFilePath, json);
        f.stopwatch("Json Serialize");
    }

    private void ReadOldBlend()
    {
        oldBlenderFile = new BlenderFileReader.BlenderFile(blendFilePath);
    }
}