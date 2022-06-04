using UnityEngine;
using UnityEditor;
using UnityEditor.AssetImporters;

namespace Blender.Importer
{
    /// <summary>
    /// Preprocess Blend Files, Used to override default asset importer.
    /// </summary>
    public class BlenderPreProcessor : AssetPostprocessor
    {
        void OnPreprocessAsset()
        {
            Debug.Log("Importing texture to: " + assetPath);
            var path = assetPath;

            if (path.Contains(".blend"))
            {
                var currentOveride = AssetDatabase.GetImporterOverride(path);
                if (currentOveride == null)
                {
                    // Set the importer to the new Blender Importer if it doesn't have one.
                    AssetDatabase.SetImporterOverride<BlendImporter>(path);
                }
            }
        }
    }
}