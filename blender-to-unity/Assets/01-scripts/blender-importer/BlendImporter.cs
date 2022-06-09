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

        public bool overrideDefaultMaterial = true;
        public Material defaultMaterial;
        
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
            CreateData(ctx,blendData);

            // 8. Build Data.
            foreach(GameObject go in goMap.Values)
            {
                go.GetComponent<MeshRenderer>().sharedMaterial = defaultMaterial;
            }
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
    


        // 7. Data

        private Dictionary<string,Mesh> meshMap = new Dictionary<string, Mesh>();
        
        private Dictionary<string,Material> matMap = new Dictionary<string, Material>();

        private Dictionary<string,GameObject> goMap = new Dictionary<string, GameObject>();

        private void CreateData(AssetImportContext ctx, BlendData blendData)
        {
            var rootObject = new GameObject(Path.GetFileNameWithoutExtension(ctx.assetPath));
            ctx.AddObjectToAsset(rootObject.name, rootObject);
            ctx.SetMainObject(rootObject);

            // Meshes
            foreach(BlendMesh bMesh in blendData.blend_meshes)
            {
                Mesh mesh = CreateMesh(bMesh);
                meshMap.Add(bMesh.name_id, mesh);
                ctx.AddObjectToAsset(mesh.name, mesh);
            }

            // Gameobjects
            foreach(BlendGameObject bGameObject in blendData.blend_gameobjects)
            {
                GameObject gameObject = CreateGameObject(bGameObject);
                goMap.Add(bGameObject.name_id, gameObject);
            }

            foreach(BlendGameObject bGameObject in blendData.blend_gameobjects)
            {
                if(bGameObject.parent_id == string.Empty)
                {
                    goMap[bGameObject.name_id].transform.parent = rootObject.transform;
                }
                else
                {
                    goMap[bGameObject.name_id].transform.parent = goMap[bGameObject.parent_id].transform;
                }
                ctx.AddObjectToAsset(bGameObject.name_id, goMap[bGameObject.name_id]);
            }
        }
        public Mesh CreateMesh(BlendMesh blendMesh)
        {
            var mesh = new Mesh();
            mesh.name = blendMesh.name_id;
            mesh.SetVertices(FloatToVector3(blendMesh.vertices));
            mesh.SetNormals(FloatToVector3(blendMesh.normals));
            mesh.subMeshCount = blendMesh.sub_meshes.Length;
            for (int i = 0; i < blendMesh.sub_meshes.Length; i++)
            {
                mesh.SetTriangles(blendMesh.sub_meshes[i].triangles, i);
            }
            return mesh;
        }
        private GameObject CreateGameObject(BlendGameObject blendGo)
        {
            var go = new GameObject(blendGo.name_id);
            go.transform.position = blendGo.position;
            go.transform.rotation = Quaternion.Euler(blendGo.rotation * Mathf.Rad2Deg);
            go.transform.localScale = blendGo.scale;

            // Materials
            // var slotNumbers = blendGo.material_slots > 0 ? blendGo.material_slots : 1; // consider no slots
            // Material[] mats = new Material[slotNumbers];
            // for (int i = 0; i < slotNumbers; i++)
            // {
            //     mats[i] = defaultMaterial;

            //     var colors = new Color[8]{Color.gray, Color.red, Color.green, Color.blue, Color.cyan, Color.magenta, Color.yellow, Color.clear};

            //     var colorIndex = i - Mathf.FloorToInt(i / 8);
            //     // rand color for fun
            //     mats[i].color = colors[colorIndex];
            // }

            // Mesh
            if(meshMap.ContainsKey(blendGo.mesh_id))
            {
                var meshFilter = go.AddComponent<MeshFilter>();
                meshFilter.mesh = meshMap[blendGo.mesh_id];
                
                var mr = go.AddComponent<MeshRenderer>();
                //mr.sharedMaterials = mats;
            }

            return go;
        }
        private Vector3[] FloatToVector3(float[] floats)
        {
            var vectors = new Vector3[floats.Length / 3];
            for (int i = 0; i < vectors.Length; i++)
            {
                vectors[i] = new Vector3(floats[i * 3], floats[i * 3 + 1], floats[i * 3 + 2]);
            }
            return vectors;
        }
        private Vector2[] FloatToVector2(float[] floats)
        {
            var vectors = new Vector2[floats.Length / 2];
            for (int i = 0; i < vectors.Length; i++)
            {
                vectors[i] = new Vector2(floats[i * 2], floats[i * 2 + 1]);
            }
            return vectors;
        }
    }

}