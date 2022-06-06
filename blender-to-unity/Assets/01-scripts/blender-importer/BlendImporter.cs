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
        [FoldoutGroup("Blend Data")]
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
            var meshesFilePath = $"{blendPath}{blendName}_meshes.json";
            var meshesJson = ReadJson(meshesFilePath);

            // 6. Deserialize Data.
            EditorJsonUtility.FromJsonOverwrite(meshesJson, blendMeshes);

            // 7. Create Data.
            // Meshes
            var meshMap = new Dictionary<string, Mesh>();
            foreach (var blendMesh in blendMeshes.meshes)
            {
                var mesh = MeshCreator.CreateMesh(blendMesh);
                meshMap.Add(blendMesh.name_id, mesh);
            }

            // GameObjects (Temp) TODO - Add temp mesh objects.
            foreach(var mesh in meshMap.Values)
            {
                var go = new GameObject();
                go.AddComponent<MeshFilter>().mesh = mesh;
                go.AddComponent<MeshRenderer>();
            }
        }

        // Get the Blend File Path relative to the project
        private string GetBlendPath(string assetPath)
        {
            var projectPath = Application.dataPath.Replace("Assets", "");
            return projectPath + assetPath.Replace(Path.GetFileName(assetPath), "");
        }

        private string GetBlendName(string assetPath)
        {
            return Path.GetFileNameWithoutExtension(assetPath);
        }

        private string ReadJson(string filePath)
        {
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