using UnityEditor;
using UnityEngine;
using Sirenix.OdinInspector;

public class BlendReaderInput : MonoBehaviour
{
    private string blendFilePath = @"E:\repos\blender-to-unity\blender-to-unity\Assets\01-scripts\blend-parser\2012\example.blend";

    public BlendHeader hdr;
    [Button]
    private void ReadBlend()
    {
        var blendFile = new BlenderFile(blendFilePath);
        hdr = blendFile.Header;
    }
}