using UnityEngine;
using UnityEditor;
using UnityEditor.AssetImporters;
using System.IO;
using System.Collections.Generic;


namespace UBlend
{
    [ScriptedImporter(1, "ublend")]
    public class Importer : ScriptedImporter
    {
        private Material debugMat;
        [ReadOnly]public Data uBlendData;

        public enum JSONSerializer { UNITY,JSON_NET, JSON_NET_OBJECT};
        [SerializeField]
        private JSONSerializer serializer = JSONSerializer.JSON_NET;

        private GameObject rootObject;
        private Dictionary<string,GameObject> GameObjectReference = new Dictionary<string,GameObject>();
        private Dictionary<string,Mesh> MeshReference = new Dictionary<string,Mesh>();
        private Dictionary<string,Material> MaterialReference = new Dictionary<string,Material>();

        public override void OnImportAsset(AssetImportContext ctx)
        {
            //TEMP
            debugMat = (Material)AssetDatabase.LoadAssetAtPath("Assets/Shaders/uv.mat", typeof(Material));

            var readTime = System.Diagnostics.Stopwatch.StartNew();
            // Read text from file.
            string fileContent = File.ReadAllText(ctx.assetPath);

            readTime.Stop();

            var parseTime = System.Diagnostics.Stopwatch.StartNew();
            
            switch(serializer)
            {
                case JSONSerializer.UNITY:
                    uBlendData = JsonUtility.FromJson<Data>(fileContent);
                    break;
                case JSONSerializer.JSON_NET:
                    uBlendData = Newtonsoft.Json.JsonConvert.DeserializeObject<Data>(fileContent);
                    break;
                case JSONSerializer.JSON_NET_OBJECT:
                    uBlendData = UBlendJsonNet_JObject.GetUBlend(fileContent);
                    break;
            }
           
            

            parseTime.Stop();

            var createTime = System.Diagnostics.Stopwatch.StartNew();

            CreateData(ctx,uBlendData);

            createTime.Stop();

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            Debug.Log($"Read: {readTime.ElapsedMilliseconds}ms");
            Debug.Log($"Parse: {parseTime.ElapsedMilliseconds}ms using {serializer}");
            Debug.Log($"Create: {createTime.ElapsedMilliseconds}ms");
        }
        public void CreateData(AssetImportContext ctx, Data uBlendData)
        {
            rootObject = new GameObject();
            ctx.AddObjectToAsset(Path.GetFileNameWithoutExtension(ctx.assetPath), rootObject);
            CreateMeshes(ctx,uBlendData.u_assets.u_meshes);
            CreateGameObjects(uBlendData.u_objects.u_gameobjects);
            SetGameObjectParents(uBlendData);
            ctx.SetMainObject(rootObject);
        }

        public void CreateMeshes(AssetImportContext ctx, List<UMesh> u_meshes)
        {
            foreach (var u_mesh in u_meshes)
            {
                Mesh mesh = new Mesh();
                mesh.name = u_mesh.u_name;
                mesh.SetVertices(u_mesh.u_vertices);
                mesh.SetNormals(u_mesh.u_normals);
                mesh.subMeshCount = u_mesh.u_submesh_count;
                for (int i = 0; i < u_mesh.u_submesh_triangles.Count; i++)
                {
                    mesh.SetTriangles(u_mesh.u_submesh_triangles[i].u_triangles, (i), true);
                }
                for (int i = 0; i < u_mesh.u_uvs.Count; i++)
                {
                    mesh.SetUVs(i, u_mesh.u_uvs[i].u_uv);
                }

                MeshReference.Add(u_mesh.u_name, mesh);

                ctx.AddObjectToAsset(mesh.name,mesh);
            }
        }

        public void CreateGameObjects(List<UGameObject> u_gameobjects)
        {
            foreach(var u_gameobject in u_gameobjects)
            {
                var go = new GameObject(u_gameobject.name); // Essential that the game object name match the u_name.
                go.transform.position = u_gameobject.u_transform.position;
                go.transform.rotation = Quaternion.Euler(u_gameobject.u_transform.rotation);
                go.transform.localScale = u_gameobject.u_transform.scale;
                CreateComponents(go, u_gameobject);

                GameObjectReference.Add(go.name,go);
            }
        }

        public void CreateComponents(GameObject go, UGameObject u_go)
        {
            foreach(var u_co in u_go.u_components)
            {
                if(u_co is UMeshFilter)
                {
                    var u_mf = u_co as UMeshFilter;
                    var mf = go.AddComponent<MeshFilter>();
                    mf.mesh = MeshReference[u_mf.mesh_name];

                    // TEMP. For now we'll assing a basic mesh renderer.
                    var mr = go.AddComponent<MeshRenderer>();
                    mr.sharedMaterial = debugMat;
                }
            }
        }

        public void SetGameObjectParents(Data uBlendData)
        {
            foreach(UGameObject ugo in uBlendData.u_objects.u_gameobjects)
            {
                // If the udata has a parent, then set the mapped gameobject.transform.parent to the udata parent.
                if(ugo.u_transform.parent_name != "")
                {
                    var go = GameObjectReference[ugo.name];
                    var goParent = GameObjectReference[ugo.u_transform.parent_name];
                    go.transform.parent = goParent.transform;
                }
                else
                {
                    // If the udata has no parent, then set the mapped gameobject.transform.parent to the rootObject.transform.
                    var go = GameObjectReference[ugo.name];
                    go.transform.parent = rootObject.transform;
                }
            }
        }
    }
}