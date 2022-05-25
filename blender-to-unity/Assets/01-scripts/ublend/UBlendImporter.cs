using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.AssetImporters;
using System.IO;
using Sirenix.OdinInspector;
using System.Diagnostics;

namespace UBlend
{
    [ScriptedImporter(1, "ublend")]
    public class UBlendImporter : ScriptedImporter
    {
        [ReadOnly]
        public UBlend uBlend;

        public override void OnImportAsset(AssetImportContext ctx)
        {
            var readTime = Stopwatch.StartNew();
            var json = File.ReadAllText(ctx.assetPath);
            readTime.Stop();

            var deserializeTime = Stopwatch.StartNew();
            EditorJsonUtility.FromJsonOverwrite(json,uBlend);
            deserializeTime.Stop();

            UnityEngine.Debug.Log($"Read time: {readTime.ElapsedMilliseconds}ms");
            UnityEngine.Debug.Log($"Deserialize time: {deserializeTime.ElapsedMilliseconds}ms");
        }
    }
}