using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json.Linq;

/*  
    Because we're creating all our data at intialisation we need to be modifying that original data for faster deserialisation.
    rather than returning new instances of the data.
*/

namespace UBlend{
    public class Ops 
    {
        public static JObject GetJObject(string json)
        {
            return JObject.Parse(json);
        }

        public static Data GetUBlend(JObject json)
        {
            Data data = new Data();

            Debug.Log($"Key of data.u_assets: {nameof(data.u_assets)}");
            GetAssets(json[nameof(data.u_assets)],data.u_assets);
            GetObjects(json[nameof(data.u_objects)],data.u_objects);

            return data;
        }

        public static void GetAssets(JToken assetsToken, UAssets assets)
        {
            assets = assetsToken.ToObject<UAssets>();
            //get meshes
            //get materials
            //get textures
        }

        public static void GetObjects(JToken objectsToken, UObjects objects)
        {
            GetGameObjects(objectsToken[nameof(objects.u_gameobjects)],objects.u_gameobjects);
        }

        public static void GetGameObjects(JToken gameobjectsToken, List<UGameObject> gameObjects)
        {
            foreach(JToken t in gameobjectsToken[nameof(UGameObject)])
            {
                var go = new UGameObject();
                go.name = t[nameof(go.name)].ToString();
                gameObjects.Add(go);
            }
        }
    }

}