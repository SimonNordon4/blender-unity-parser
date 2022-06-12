using UnityEditor;
using UnityEngine;
using Sirenix.OdinInspector;

[ExecuteInEditMode]
public class BlendReaderInput : MonoBehaviour
{
    
    private string blendFilePath = @"E:\repos\blender-to-unity\blender-to-unity\Assets\01-scripts\blend-parser\2012\example.blend";

    [ReadOnly]
    public BlenderFile blend;

    [Button]
    private void ReadBlend()
    {
        var blendFile = new BlenderFile(blendFilePath);
        blend = blendFile;
    }
}