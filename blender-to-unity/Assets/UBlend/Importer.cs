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
        private GameObject rootObject;
        private Dictionary<string,GameObject> GameObjectReference = new Dictionary<string,GameObject>();
        private Dictionary<string,Mesh> MeshReference = new Dictionary<string,Mesh>();
        private Dictionary<string,Material> MaterialReference = new Dictionary<string,Material>();

        [ReadOnly]public Data uBlendData;
        public override void OnImportAsset(AssetImportContext ctx)
        {
            // Read text from file.
            string fileContent = File.ReadAllText(ctx.assetPath);
            
            // Convert to UBlendData.
            uBlendData = ReadUBlend.GetUBlend(ReadUBlend.GetJObject(fileContent));

            CreateData(ctx,uBlendData);

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
        public void CreateData(AssetImportContext ctx, Data uBlendData)
        {
            rootObject = new GameObject();
            rootObject.AddComponent<UBlendComponent>().uBlendData = uBlendData;
            ctx.AddObjectToAsset(Path.GetFileNameWithoutExtension(ctx.assetPath), rootObject);
            CreateGameObjects(ctx,uBlendData);
            SetGameObjectParents(uBlendData);
            ctx.SetMainObject(rootObject);
        }

        public void CreateMeshes(AssetImportContext ctx, Data uBlendData)
        {
            // TODO: Implement
        }

        public void CreateGameObjects(AssetImportContext ctx, Data uBlendData)
        {
            foreach(var u_gameobject in uBlendData.u_objects.u_gameobjects)
            {
                var go = new GameObject(u_gameobject.name); // Essential that the game object name match the u_name.
                go.transform.position = u_gameobject.u_transform.position;
                go.transform.rotation = Quaternion.Euler(u_gameobject.u_transform.rotation);
                go.transform.localScale = u_gameobject.u_transform.scale;
                CreateComponents(go, u_gameobject);

                GameObjectReference.Add(go.name,go);

                ctx.AddObjectToAsset(go.name, go);
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
                    //mf.mesh = MeshReference[u_mf.mesh_name];

                    // TEMP. For now we'll assing a basic mesh renderer.
                    var mr = go.AddComponent<MeshRenderer>();
                    mr.material = new Material(Shader.Find("Universal Render Pipeline/Lit"));
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