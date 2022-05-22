using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json.Linq;

/*  
    Because we're creating all our data at intialisation we need to be modifying that original data for faster deserialisation.
    rather than returning new instances of the data.
*/

namespace UBlend
{
    public class Ops
    {
        public const string k_type = "type";
        public const string name_space = "UBlend.";

        public static JObject GetJObject(string json)
        {
            return JObject.Parse(json);
        }

        public static Data GetUBlend(JObject json)
        {
            Data data = new Data();

            GetAssets(json[nameof(data.u_assets)], data.u_assets);
            GetObjects(json[nameof(data.u_objects)], data.u_objects);

            return data;
        }

        public static void GetAssets(JToken u_assetsToken, UAssets u_assets)
        {

            u_assets = u_assetsToken.ToObject<UAssets>();
            //get meshes
            //get materials
            //get textures
        }

        public static void GetObjects(JToken u_objectsToken, UObjects u_objects)
        {
            GetGameObjects(u_objectsToken[nameof(u_objects.u_gameobjects)], u_objects.u_gameobjects);
        }

        public static void GetGameObjects(JToken u_gameobjectsToken, List<UGameObject> u_gameobjects)
        {
            foreach (JToken t in u_gameobjectsToken[nameof(UGameObject)])
            {
                var go = new UGameObject();
                go.name = t[nameof(go.name)].ToString();
                GetComponents(t[nameof(go.u_components)], go.u_components);
                u_gameobjects.Add(go);
            }
        }

        public static void GetComponents(JToken u_componentsToken, List<UComponent> u_components)
        {
            foreach (JToken t in u_componentsToken[nameof(UComponent)])
            {
                Type uType = Type.GetType(name_space + t[k_type].ToString());

                if(uType == typeof(UTransform))
                {
                   
                }
            }
        }

        public static bool CheckKey(string key, JToken token)
        {
            if (token[key] is null)
            {
                Debug.LogError($"{key} is not a valid key in {token} ");
                return false;
            }
            return true;
        }
    }



}