using UnityEngine;
using UnityEditor;
using UnityEditor.AssetImporters;

/// <summary>
/// Preprocess Blend Files, Used to override default asset importer.
/// </summary>
public class BlendPreProcessor : AssetPostprocessor
{
    void OnPreprocessAsset()
    {
        Debug.Log("Importing texture to: " + assetPath);
        var path = assetPath;

        if (path.Contains(".blend"))
        {
            var currentOveride = AssetDatabase.GetImporterOverride(path);
            if (currentOveride == null)
            {
                // Set the importer to the new Blender Importer if it doesn't have one.
                AssetDatabase.SetImporterOverride<BlendImporter>(path);
            }
        }
    }

}
//This is Example Importer for cube
[ScriptedImporter(1, new[] { "cube1" }, new[] { "blend" })]
public class BlendImporter : ScriptedImporter
{
    public override void OnImportAsset(AssetImportContext ctx)
    {
        Debug.Log("This is completely new BLENDER importer!");
        var cube = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        cube.transform.position = new Vector3(0, 0, 0);
        // 'cube' is a GameObject and will be automatically converted into a prefab
        ctx.AddObjectToAsset("main obj", cube);
        ctx.SetMainObject(cube);
    }
}

