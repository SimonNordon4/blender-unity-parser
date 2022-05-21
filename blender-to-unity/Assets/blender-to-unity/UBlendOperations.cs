using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Newtonsoft.Json.Linq;

namespace UnityToBlender
{
    /// <summary>
    /// Standard Operations for creation of unity objects.
    /// </summary>
    public class UBlendOperations
    {
        public static UBlendData JObjectToUBlendData(JObject jo)
        {
            var uBlendData = new UBlendData();

            uBlendData.uGameObjects = GetUGameObjects(jo);

            return uBlendData;
        }

        public static List<UGameObject> GetUGameObjects(JObject jo)
        {
            List<UGameObject> uGameObjects = new List<UGameObject>();

            foreach (var uGameObjectToken in jo["u_gameobjects"])
            {
                var uGameObject = new UGameObject();
                uGameObject.id = uGameObjectToken["id"].ToString();
                uGameObject.name = uGameObjectToken["name"].ToString();
                uGameObject.uComponents = GetUComponents(uGameObjectToken);
                uGameObjects.Add(uGameObject);
            }

            return uGameObjects;
        }

        public static List<UComponent> GetUComponents(JToken uGameObjectToken)
        {

            List<UComponent> uComponents = new List<UComponent>();
            foreach (var uComponentToken in uGameObjectToken["components"])
            {
                var typeDefintion = "UnityToBlender." + uComponentToken["type"].ToString();
                var uComponent = GetUComponent(uComponentToken,Type.GetType(typeDefintion));
                uComponents.Add(uComponent);
            }

            return uComponents;
        }

        public static UComponent GetUComponent(JToken uComponentToken, Type type)
        {   
            //hack
            if(type == typeof(UTransform))
            {
                var UTransform = new UTransform();
                UTransform.id = uComponentToken["id"].ToString();
                UTransform.type = type;
                UTransform.position = new Vector3(float.Parse(uComponentToken["position"]["x"].ToString()), float.Parse(uComponentToken["position"]["y"].ToString()), float.Parse(uComponentToken["position"]["z"].ToString()));
                UTransform.rotation = new Vector3(float.Parse(uComponentToken["rotation"]["x"].ToString()), float.Parse(uComponentToken["rotation"]["y"].ToString()), float.Parse(uComponentToken["rotation"]["z"].ToString()));
                UTransform.scale = new Vector3(float.Parse(uComponentToken["scale"]["x"].ToString()), float.Parse(uComponentToken["scale"]["y"].ToString()), float.Parse(uComponentToken["scale"]["z"].ToString()));
                return UTransform;
            }
            return default;
        }
    }
}