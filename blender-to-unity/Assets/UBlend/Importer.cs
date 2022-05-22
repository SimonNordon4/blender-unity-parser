using UnityEngine;
using UnityEditor;
using UnityEditor.AssetImporters;
using System.IO;


namespace UBlend
{
    [ScriptedImporter(1, "ublend")]
    public class Importer : ScriptedImporter
    {
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
            var ubd_go = new GameObject();
            ubd_go.AddComponent<UBlendComponent>().uBlendData = uBlendData;
            ctx.AddObjectToAsset(Path.GetFileNameWithoutExtension(ctx.assetPath), ubd_go);
            ctx.SetMainObject(ubd_go);
        }

        public void CreateMeshes(AssetImportContext ctx, Data uBlendData)
        {
            // TODO: Implement
        }

        public void CreateGameObjects(AssetImportContext ctx, Data uBlendData)
        {
            foreach(var u_gameobject in uBlendData.u_objects.u_gameobjects)
            {
                var go = new GameObject(u_gameobject.name);
                // go.transform.position =
                // go.transform.rotation = 
                // go.transform.lossyScale = 
                
                // ctx.AddObjectToAsset(u_object.name, u_object_go);
                // ctx.SetMainObject(u_object_go);
            }
        }
    }
}