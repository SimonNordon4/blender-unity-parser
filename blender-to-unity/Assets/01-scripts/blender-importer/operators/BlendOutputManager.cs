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
        public void GetOutputs(string path, string name)
        {
            var meshes_path = $"{path}{name}_meshes.json";

            var materials_path = $"{path}{name}_materials.json";

            var textures_path = $"{path}{name}_textures.json";

            var gameobjects_path = $"{path}{name}_gameobjects.json";
        }

        private string ReadJson(string path)
        {
            var json = File.ReadAllText(path);
            return json;
        }
    }

    // Add path, json output here.
    public class BlendOutputData
    {
        public string name;
        // returns false if no json data can be found.
        public bool isValid;
        public string path;
        public string json;

    }
}