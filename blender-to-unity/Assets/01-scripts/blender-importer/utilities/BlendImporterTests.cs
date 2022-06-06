using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Sirinex.OdinInspector;

public class BlendImporterTests : MonoBehaviour
{
    // 1. Overwrite File Import
        // 1.1 Ignore Blend1 Files.

    [Button]
    public void Test_OverWriteFileImport()
    {

    }

    // 2. Import File Path.

    // 3. Load Default Import Settings.
        // 3.1 Global Settings Asset.
        // 3.2 Overwritten Import Settings

    // 4. Retrieve Blender Executable Path.
        // 4.1 Load from Global Settings.
        // 4.2 If Empty, try to find on System and add to Global Settings.
        // 4.3 If Still Empty, Ask User to Set it.

    // 5. Retrieve Python Executable Path
        // 5.1 Load Python Exectuable from Global Settings Asset
        // 5.2 If Empty, try to find on System and add to Global Settings.
        // 5.3 If Still Empty, Ask User to Set it.

    // 6. Compile Python Args
        // 6.1 Location of Python Modules.
        // 6.2 .blend path (for exporting to the same path)
        // 6.3 .blend name (for exporting to the same name)
        // 6.4 Importer Settings.

    // 7. Run Blender Process.
        // 7.1 Error Handling.

    // 8. Export JSON Outputs.
        // 8.1 blend_meshes.json
        // 8.2 blend_materials.json
        // 8.3 blend_textures.json
        // 8.4 blend_collections.json
        // 8.5 blend_objects.json

    // 9. Deserialize JSON Outputs.
        // 9.1 blend_meshes.json to BlendMeshes
        // 9.2 blend_materials.json to BlendMaterials
        // 9.3 blend_textures.json to BlendTextures
        // 9.4 blend_collections.json to BlendCollections
        // 9.5 blend_objects.json to BlendObjcts

    // 10. Generate Unity Objects
        // 10.1 BlendMeshes to Unity Meshes
        // 10.2 BlendMaterials to Unity Materials
        // 10.3 BlendTextures to Unity Textures
        // 10.4 BlendCollections to Unity GameObjects
        // 10.5 BlendObjects to Unity GameObjects
}
