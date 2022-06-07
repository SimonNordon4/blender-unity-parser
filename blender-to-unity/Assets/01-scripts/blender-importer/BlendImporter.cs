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
using System.Reflection;

namespace Blender.Importer
{
    //This is Example Importer for cube
    [ScriptedImporter(1, new[] { "lol" }, new[] { "blend" })]
    public class BlendImporter : ScriptedImporter
    {
        [FoldoutGroup("Blend Data")]
        [ReadOnly]
        public BlendMeshes blendMeshes;

        private string blend_path;
        private string blend_name;
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
            BlendOutputManager.GetOutputs(blend_path, blend_name);

            // 6. Deserialize Data.
            // TODO: Foreach in BledOutPutManager.
            //EditorJsonUtility.FromJsonOverwrite(meshes_data.json, blendMeshes);

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

        //3. Compile Import Arguments
        private string ResolveArguments(string assetPath)
        {
            var args = string.Empty;
            blend_path = GetBlendPath(assetPath);
            blend_name = GetBlendName(assetPath);
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