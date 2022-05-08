using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.AssetImporters;
using System.IO;
using Newtonsoft.Json;
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
        ImportMesh(blob);
        
    }

    private void ImportMesh(string blob)
    {
        var meshJson = DataToDictionary(blob);
        var name = meshJson["name"];

        var vertices = GetVector3FromData(meshJson["vertices"].ToString());
    }

    private Vector3[] GetVector3FromData(string vertices)
    {
        var vector3Datas = DataToDictionary(vertices);

        for (int i = 0;i < vector3Datas.Count; i++)
        {
            var vector3Data = DataToArray<double>(vector3Datas[i.ToString()].ToString());
            Debug.Log(vector3Data);
        }
        
        return null;
    }

    public Dictionary<string, object> DataToDictionary(string data)
    {
        return JsonConvert.DeserializeObject<Dictionary<string, object>>(data);
    }

    public T[] DataToArray<T>(string data)
    {
        return JsonConvert.DeserializeObject<T[]>(data);
    }
}






