using System;
using UnityEngine;
using UnityEditor;
using UnityEditor.AssetImporters;


public class AssetDatabaseExamples : MonoBehaviour
{
    [MenuItem("AssetDatabase/Available Importer Types for cube")]
    static void AvailableImporterTypeCube()
    {
        var path = "Assets/01-scripts/blender-importer/cube1.blend";
        // returns null because no Override Importer is set.
        AssetDatabase.ClearImporterOverride(path);
        // returns [CubeImporter].
        AssetDatabase.SetImporterOverride<CubeImporter>(path);
    }

    //This is Example Importer for cube
    [ScriptedImporter(1, new []{"cube" }, new[] { "blend" })]
    public class CubeImporter : ScriptedImporter
    {
        public override void OnImportAsset(AssetImportContext ctx)
        {
            Debug.Log("This is completely new importer!");
            var cube = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            cube.transform.position = new Vector3(0, 0, 0);
            // 'cube' is a GameObject and will be automatically converted into a prefab
            ctx.AddObjectToAsset("main obj", cube);
            ctx.SetMainObject(cube);
        }
    }
}