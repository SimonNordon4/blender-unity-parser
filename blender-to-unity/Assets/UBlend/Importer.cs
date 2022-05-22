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
            // Read text from file.
            string fileContent = File.ReadAllText(ctx.assetPath);
            
            // Convert to UBlendData.
            uBlendData = Ops.GetUBlend(Ops.GetJObject(fileContent));

            // Convert uBlendData to Unity Data.
        }
    }
}