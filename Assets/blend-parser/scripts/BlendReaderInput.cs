using UnityEditor;
using UnityEngine;
using BlenderToUnity;

#if UNITY_EDITOR
[ExecuteInEditMode]
public class BlendReaderInput : MonoBehaviour
{
    [SerializeField]
    private string blendFilePath = @"Assets/blend-parser/BlenderToUnity/example.blend";
    [SerializeField]
    private string jsonOutputPath = @"Assets/blend-parser/BlenderToUnity/example.json";
    [field:SerializeField]
    public BlenderFile BlenderFile { get; private set; }

    [ContextMenu("Read Blend File")]
    public void ReadBlend()
    {
        // Make the path relative to the project
        var absoluteBlendFilePath = blendFilePath.Replace("Assets",Application.dataPath);
        print("full path: " + absoluteBlendFilePath);
        BlenderFile = new BlenderFile(absoluteBlendFilePath);
    }

    public void SaveBlendOutput()
    {
        
        if (BlenderFile == null)
        {
            Debug.LogError("No blend file has been read yet.");
            return;
        }
        var fullPath = jsonOutputPath.Replace("Assets",Application.dataPath);
        var json = JsonUtility.ToJson(BlenderFile);
        System.IO.File.WriteAllText(fullPath, json);
        Debug.Log("Blend file saved to: " + fullPath); 
        // Save the asset
        AssetDatabase.Refresh();
    }

    public void FlushBlenderFile()
    {
        BlenderFile = null;
    }
}


[CustomEditor(typeof(BlendReaderInput))]
public class BlendReaderInputEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        var blendReaderInput = (BlendReaderInput)target;
        if (GUILayout.Button("Read Blend File"))
        {
            blendReaderInput.ReadBlend();
        }
        
        if (GUILayout.Button("Save Blend Output"))
        {
            blendReaderInput.SaveBlendOutput();
        }
        
        if (GUILayout.Button("Flush Blend File"))
        {
            blendReaderInput.FlushBlenderFile();
        }
    }
}
#endif