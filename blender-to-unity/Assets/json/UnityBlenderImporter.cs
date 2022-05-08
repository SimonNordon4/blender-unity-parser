using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.AssetImporters;
using System.IO;
using TinyJson;
using Unity.VisualScripting;
using Object = UnityEngine.Object;


[ScriptedImporter(1, "ublend")]
public class UnityBlenderImporter : ScriptedImporter
{
    public override void OnImportAsset(AssetImportContext ctx)
    {
        Debug.Log($"Importing {ctx.assetPath}");
        var fileName = System.IO.Path.GetFileNameWithoutExtension(ctx.assetPath);

        var rawData = (File.ReadAllText(ctx.assetPath));
        var parser = new UBlenderParser();
        parser.ParseUBlend(rawData);
    }
}

public class UBlenderParser
{
    public void ParseUBlend(string rawData)
    {
        var x = rawData.Split("name");
        foreach (var result in x)
        {
            Debug.Log(result);
        }
    }
}

[Serializable]
public class UMesh
{
    public string name;
    //public string vertices;
    public UVertices[] vertices;
}

[Serializable]
public class UVertices
{
    public UCoordinate[] co;
}

[Serializable]
public class UCoordinate
{
    public float[] xyz;
}








