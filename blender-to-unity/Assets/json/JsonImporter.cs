using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.AssetImporters;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine.UIElements;
using Object = UnityEngine.Object;

[ScriptedImporter(1,"ublend")]
public class JsonImporter : ScriptedImporter
{
    public override void OnImportAsset(AssetImportContext ctx)
    {
        Debug.Log( $"Importing {ctx.assetPath}");
        var fileName = System.IO.Path.GetFileNameWithoutExtension(ctx.assetPath);

        var blob = (File.ReadAllText(ctx.assetPath));
        var jsonObject = JsonConvert.DeserializeObject<Dictionary<string, object>>(File.ReadAllText(ctx.assetPath));

        var name = (string)jsonObject["name"];

        Debug.Log(jsonObject["name"]);
        Debug.Log(jsonObject["vertices"]);
        Debug.Log(jsonObject["normals"]);
        Debug.Log(jsonObject["triangles"]);


    }
}






