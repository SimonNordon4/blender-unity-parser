using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.AssetImporters;
using System.Diagnostics;
using System.IO;
using Sirenix.OdinInspector;
using UBlend;

namespace Blender.Importer
{

    //This is Example Importer for cube
    [ScriptedImporter(1, new[] { "cube1" }, new[] { "blend" })]
    public class BlendImporter : ScriptedImporter
    {
        #region Import Args
        public string BlendPath = string.Empty;
        public string BlendName = string.Empty;
        public bool ImportCollectionsAsObjects = false;
        public bool EmbedMaterialsAndTextures = false;
        #endregion
        private string ExportFilePath = string.Empty;

        //TODO move this scriptable object settings.
        public UBlendData m_blend;
        private string pythonExectuablePath = @"E:\repos\blender-to-unity\blender-to-unity\Assets\01-scripts\blender-importer\python\get_blend_data.py";

        public override void OnImportAsset(AssetImportContext ctx)
        {
            // 0. Load Default Settings.
            SetImportVariables(ctx);
            // 1. Retrive Blender Executable Path on System.
                // 1.1 If one can not be found, ask user to set it, and save it in a global import settings scriptable object.
                // 1.2 Verify it's an actualy .exe file.
                // 1.3 Ensure the supplied Blender Version is supported.
            var blenderExectuablePath = GetBlenderExecutablePath();
            var args = $"{BlendPath} {BlendName} {ImportCollectionsAsObjects} {EmbedMaterialsAndTextures}";

            // 2. Compile Import Arguments.

            // 3. Run Blender Process.
            var result = BlenderProcessHandler.RunBlender(blenderExectuablePath, pythonExectuablePath, ctx.assetPath, args);
            f.print(result);
            // 4. Deserialize the exported JSON Data.
                //4.1 Ensure the JSON is valid.
            DeserializeExportedJson();

            // 5. Create Meshes.

            // 6. Create Textures.

            // 7. Create Materials.

            // 8. Create GameObjects.

            // 9. Create Hierarchy.
            var name = Path.GetFileNameWithoutExtension(ctx.assetPath);
            var rootGameObject = new GameObject(name);


            var tempImport = new UBlendImporter();
            tempImport.CreateMeshes(ctx, m_blend);
            // tempImport.CreateTextures(ctx, m_blend);
            // tempImport.CreateMaterials(ctx, m_blend);
            tempImport.CreateGameObjects(ctx, m_blend);
            tempImport.CreateHierachy(m_blend,rootGameObject.transform);
            ctx.AddObjectToAsset(name, rootGameObject);
            ctx.SetMainObject(rootGameObject);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        private string GetBlenderExecutablePath()
        {
            return @"C:\Program Files\Blender Foundation\Blender 3.1\blender.exe";
        }

        private void SetImportVariables(AssetImportContext ctx)
        {
            var blendPath = ctx.assetPath;
            var fullPath = Path.GetFullPath(blendPath);
            BlendPath = fullPath.Replace(Path.GetFileName(blendPath), "");
            BlendName = Path.GetFileNameWithoutExtension(blendPath);
            ExportFilePath = fullPath.Replace(".blend", ".json");
            f.print(ExportFilePath);
        }

        private void OnBlenderImported(string result)
        {

        }

        private void DeserializeExportedJson()
        {
            EditorJsonUtility.FromJsonOverwrite(File.ReadAllText(ExportFilePath), m_blend);
            File.Delete(ExportFilePath);
            File.Delete(ExportFilePath + ".meta");
            AssetDatabase.Refresh();
        }

    }
}
