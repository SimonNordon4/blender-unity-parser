using UnityEditor;
using UnityEngine;
using BlenderToUnity;

[ExecuteInEditMode]
public class BlendReaderInput : MonoBehaviour
{
    
    private string blendFilePath = @"E:\repos\blender-to-unity\blender-to-unity\Assets\01-scripts\blend-parser\BlenderToUnity\example.blend";
    private string saveFilePath = @"E:\repos\blender-to-unity\blender-to-unity\Assets\01-scripts\blend-parser\BlenderToUnity\example.json";

    private BlenderFileReader.BlenderFile oldBlenderFile;

    public BlenderFile blenderFile;

    [ContextMenu("Read Blend File")]
    public void ReadBlend()
    {
        blenderFile = new BlenderFile(blendFilePath);
    }

    public void SaveBlend()
    {
        f.startwatch("Json Serialize");
        var json = EditorJsonUtility.ToJson(blenderFile, true);
        System.IO.File.WriteAllText(saveFilePath, json);
        f.stopwatch("Json Serialize");
    }

    public void ReadOldBlend()
    {
        oldBlenderFile = new BlenderFileReader.BlenderFile(blendFilePath);
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(BlendReaderInput))]
public class BlendReaderInputEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        BlendReaderInput blendReaderInput = (BlendReaderInput)target;
        if (GUILayout.Button("Read Blend File"))
        {
            blendReaderInput.ReadBlend();
        }
        if (GUILayout.Button("Save Blend File"))
        {
            blendReaderInput.SaveBlend();
        }
        if (GUILayout.Button("Read Old Blend File"))
        {
            blendReaderInput.ReadOldBlend();
        }
    }
}
#endif