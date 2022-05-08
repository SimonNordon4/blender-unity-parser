using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.AssetImporters;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;


[ScriptedImporter(1, "ublend")]
public class UnityBlenderImporter : ScriptedImporter
{
    public override void OnImportAsset(AssetImportContext ctx)
    {
        Debug.Log($"Importing {ctx.assetPath}");
        var fileName = System.IO.Path.GetFileNameWithoutExtension(ctx.assetPath);

        var json = (File.ReadAllText(ctx.assetPath));
        var obj = JsonConvert.DeserializeObject<IDictionary<string, object>>(
            json, new JsonConverter[] {new DictionaryReader()});

        foreach (var key in obj.Keys)
        {
            Debug.Log(key);
        }

        Dictionary<string, object> verticesDict = (Dictionary<string, object>)obj["vertices"];
        
        foreach (var key in verticesDict.Keys)
        {
            Debug.Log(key);
            Debug.Log(verticesDict[key]);
        }

    }
}


class DictionaryReader : CustomCreationConverter<IDictionary<string, object>>
{
    public override IDictionary<string, object> Create(Type objectType)
    {
        return new Dictionary<string, object>();
    }

    public override bool CanConvert(Type objectType)
    {
        // in addition to handling IDictionary<string, object>
        // we want to handle the deserialization of dict value
        // which is of type object
        return objectType == typeof(object) || base.CanConvert(objectType);
    }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
        if (reader.TokenType == JsonToken.StartObject
            || reader.TokenType == JsonToken.Null)
            return base.ReadJson(reader, objectType, existingValue, serializer);

        // if the next token is not an object
        // then fall back on standard deserializer (strings, numbers etc.)
        return serializer.Deserialize(reader);
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








