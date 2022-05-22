using UnityEngine;
using UnityEditor.AssetImporters;
using System.IO;


namespace UBlend
{
    [ScriptedImporter(1, "ublend")]
    public class Importer : ScriptedImporter
    {
        [ReadOnly]public Data uBlendData;
        public override void OnImportAsset(AssetImportContext ctx)
        {
            string fileContent = File.ReadAllText(ctx.assetPath);
            //JObject jObject = JObject.Parse(json);
            
            //uBlend = UBlendOperations.JObjectToUBlendData(jObject);
        }
    }
}