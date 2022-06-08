using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.AssetImporters;
using System.Diagnostics;
using System.IO;
using UBlend;
using Sirenix.OdinInspector;
using System.Reflection;

namespace Blender.Importer
{
    //This is Example Importer for cube
    [ScriptedImporter(1, new[] { "lol" }, new[] { "blend" })]
    public class BlendImporter : ScriptedImporter
    {
        [FoldoutGroup("Blend Data")]
        [ReadOnly]
        public BlendData blendData;
        [SerializeField]
        private float vec_precision = 0.001f; // higher numbers mean more compression
        
        public override void OnImportAsset(AssetImportContext ctx)
        {
            // 0. Load Default Settings.

            // 1. Retrive Blender Executable.
            var blendExe = BlendImporterGlobalSettings.instance.BlenderExectuablePath;

            // 2. Get Python Executable.
            var py = BlendImporterGlobalSettings.instance.PythonMainFile;

            // 3. Compile Import Arguments.
            var args = ResolveArguments(ctx.assetPath);

            // 4. Run Blender Process.
            BlenderProcessHandler.RunBlender(blendExe, py, ctx.assetPath, args);

            // 5. Get blender json datas.
            var json = ReadJson(ctx.assetPath);

            // 6. Deserialize Data.
            EditorJsonUtility.FromJsonOverwrite(json, blendData);

            // 7. Create Data.


            // 8. Build Data.
        }

        //3. Compile Import Arguments
        private string ResolveArguments(string assetPath)
        {
            var args = string.Empty;
            var blend_path = GetBlendPath(assetPath);
            var blend_name = GetBlendName(assetPath);
            args += $" {nameof(blend_path)}={blend_path}";
            args += $" {nameof(blend_name)}={blend_name}";
            args += $" {nameof(vec_precision)}={vec_precision}";
            return args;
        }

        private string GetBlendPath(string assetPath)
        {
            var projectPath = Application.dataPath.Replace("Assets", "");
            return projectPath + assetPath.Replace(Path.GetFileName(assetPath), "");
        }

        private string GetBlendName(string assetPath)
        {
            return Path.GetFileNameWithoutExtension(assetPath);
        }

        //5. Get Blender Outputs
        private string ReadJson(string assetPath)
        {
            var blendPath = GetBlendPath(assetPath);
            var blendName = GetBlendName(assetPath);
            var filePath = $"{blendPath}{blendName}.json";
            if(File.Exists(filePath))
            {
                return File.ReadAllText(filePath);
            }
            else
            {
                f.printError($"Tried to load {filePath} \n but it does not exist.");
                return "";
            }
        }
    }
}