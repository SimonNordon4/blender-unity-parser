using UnityEditor;
using System.IO;

namespace Blender.Importer
{
    /// <summary>
    /// Manage Blender Json Exports
    /// </summary>
    public class BlendOutputManager
    {
        

        /// <summary>
        /// Find all output json file from the blend directory [meshes,materials,textures,gameobjects]
        /// </summary>
        /// <param name="path"></param>
        /// <param name="name"></param>
        public static void GetOutputs(string path, string name)
        {
            var meshes_path = $"{path}{name}_meshes.json";

            var materials_path = $"{path}{name}_materials.json";

            var textures_path = $"{path}{name}_textures.json";

            var gameobjects_path = $"{path}{name}_gameobjects.json";
        }
    }

    // Add path, json output here.
    public class BlendOutputData
    {

    }
}