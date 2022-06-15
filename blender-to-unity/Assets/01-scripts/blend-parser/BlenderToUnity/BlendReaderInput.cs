using UnityEditor;
using UnityEngine;
using Sirenix.OdinInspector;

[ExecuteInEditMode]
public class BlendReaderInput : MonoBehaviour
{
    
    private string blendFilePath = @"E:\repos\blender-to-unity\blender-to-unity\Assets\01-scripts\blend-parser\BlenderToUnity\example.blend";


    [Button]
    private void ReadBlend()
    {
        var blendFile = new BlenderToUnity.BlenderFile(blendFilePath);
    }
}