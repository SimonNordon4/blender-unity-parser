using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.AssetImporters;
using System.Diagnostics;
using System.IO;
using UBlend;
using Blender.Importer.Operators;
using Sirenix.OdinInspector;

namespace Blender.Importer
{
    //This is Example Importer for cube
    [ScriptedImporter(1, new[] { "lol" }, new[] { "blend" })]
    public class BlendImporter : ScriptedImporter
    {
        [ReadOnly]
        public BlendMeshes blendMeshes;

        public override void OnImportAsset(AssetImportContext ctx)
        {
            // 0. Load Default Settings.

            // 1. Retrive Blender Executable.
            var blendExe = BlendImporterGlobalSettings.instance.BlenderExectuablePath;

            // 2. Get Python Executable.
            var py = BlendImporterGlobalSettings.instance.PythonMainFile;

            // 3. Compile Import Arguments.
            var blendPath = GetBlendPath(ctx.assetPath);
            var blendName = GetBlendName(ctx.assetPath);
            var args = $"{blendPath} {blendName}";

            // 4. Run Blender Process.
            BlenderProcessHandler.RunBlender(blendExe, py, ctx.assetPath, args);

            // 5. Get Blender Outputs
            var meshesJson = $"{blendPath}\\{blendName}_meshes.json";
            f.print(meshesJson);
        }

        // Get the Blend File Path relative to the project
        private string GetBlendPath(string assetPath)
        {
            return Application.dataPath + assetPath.Replace(Path.GetFileName(assetPath), "");
        }

        private string GetBlendName(string assetPath)
        {
            return Path.GetFileNameWithoutExtension(assetPath);
        }
    }
}