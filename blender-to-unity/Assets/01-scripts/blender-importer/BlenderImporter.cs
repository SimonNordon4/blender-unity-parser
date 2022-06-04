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
    public bool ImportCollectionsAsObjects = false;
    public bool EmbedMaterialsAndTextures = false;

    public BlendImporter()
    {
        BlenderProcessHandler.OnBlenderProcessFinished += this.OnBlenderImported;
    }

    ~BlendImporter()
    {
        BlenderProcessHandler.OnBlenderProcessFinished -= this.OnBlenderImported;
    }

    #endregion
    [ReadOnly] //TODO move this scriptable object settings.
    public UBlend.UBlend m_blend = new UBlend.UBlend();
    private string blenderExectuablePath = "";
    private string pythonExectuablePath = @"E:\repos\blender-to-unity\blender-to-unity\Assets\01-scripts\blender-importer\python\get_blend_data.py";
    private string pythonExport = @"E:\\repos\\blender-to-unity\\blender-to-unity\\Assets\\01-scripts\\blender-importer\\blender_export.json";


    public override void OnImportAsset(AssetImportContext ctx)
    {
        // 1. Retrive Blender Executable Path on System.
            // 1.1 If one can not be found, ask user to set it, and save it in a global import settings scriptable object.
            // 1.2 Verify it's an actualy .exe file.
            // 1.3 Ensure the supplied Blender Version is supported.
        blenderExectuablePath = GetBlenderExecutablePath();

        // 2. Compile Import Arguments.

        // 3. Run Blender Process.
        BlenderProcessHandler.RunBlender(blenderExectuablePath, pythonExectuablePath, ctx.assetPath, "true false true false yes no 1 0");

        // 4. Deserialize the exported JSON Data.

        // 5. Create Meshes.
        
        // 6. Create Textures.

        // 7. Create Materials.

        // 8. Create GameObjects.

        // 9. Create Hierarchy.
        var go = new GameObject("BlenderImporter");
        ctx.AddObjectToAsset("BlenderImporter", go);
        ctx.SetMainObject(go);
    }

    private string GetBlenderExecutablePath()
    {
        return @"C:\Program Files\Blender Foundation\Blender 3.1\blender.exe";
    }

    private void OnBlenderImported(string result)
    {
        f.print("blender imported");
        f.print(result);
        ReadBlenderOutput();
    }

    private void ReadBlenderOutput()
    {
        EditorJsonUtility.FromJsonOverwrite(File.ReadAllText(pythonExport),m_blend);

        foreach (var v in m_blend.u_meshes[0].vertices)
        {
            f.print(v.x + " " + v.y + " " + v.z);
        }
        File.Delete(pythonExport);
        AssetDatabase.Refresh();
    }

}
}
