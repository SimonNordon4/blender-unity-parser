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

            GetAssets(json["assets"],data.assets);
            GetObjects(json["objects"],data.objects);

            return data;
        }

        public static void GetAssets(JToken assetsToken, Assets assets)
        {
            assets = assetsToken.ToObject<Assets>();
            // get meshes
            // get materials
            // get textures
        }

        public static void GetObjects(JToken objectsToken, UObjects objects)
        {
            GetGameObjects(objectsToken["u_gameobjects"],objects.u_gameobjects);
        }

        public static void GetGameObjects(JToken gameobjectsToken, List<UGameObject> gameObjects)
        {
            foreach(JToken t in gameobjectsToken["GameObject"])
            {
                var go = new UGameObject();
                go.name = t["name"].ToString();
                gameObjects.Add(go);
            }
        }
    }

}