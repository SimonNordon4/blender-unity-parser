using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.AssetImporters;

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
