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
        var obj = JsonConvert.DeserializeObject<uMesh>(json);

        Debug.Log(obj.Name);
        
        //FINALLY THIS RETURNS WHAT IT'S MEANT TO
        foreach (var key in obj.Vertices.Keys)
        {
            Debug.Log(key);
            var x = obj.Vertices[key];
            foreach (var co in x)
            {
                Debug.Log(co);
            }
        }

    }
}

 public partial class uMesh
 {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("vertices")]
        public Dictionary<string, long[]> Vertices { get; set; }

        [JsonProperty("normals")]
        public Dictionary<string, long[]> Normals { get; set; }

        [JsonProperty("uv")]
        public Uv Uv { get; set; }

        [JsonProperty("triangles")]
        public Dictionary<string, long[]> Triangles { get; set; }
    }

public class UVertices
{
    public int[] Verts { get; set; }
}

    public partial class Uv
    {
    }






