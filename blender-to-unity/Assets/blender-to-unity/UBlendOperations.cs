using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace BlenderToUnity
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

            foreach (var uGameObjectToken in jo[UBlendData.uGameObjectsKey])
            {
                var uGameObject = new UGameObject();
                uGameObject.uName = uGameObjectToken[UGameObject.uNameKey].ToString();
                uGameObject.uComponents = GetUComponents(uGameObjectToken);
                uGameObjects.Add(uGameObject);
            }

            return uGameObjects;
        }

        public static List<UComponent> GetUComponents(JToken uGameObjectToken)
        {

            List<UComponent> uComponents = new List<UComponent>();
            foreach (var uComponentToken in uGameObjectToken[UGameObject.uComponentsKey])
            {
                var typeDefintion = "BlenderToUnity." + uComponentToken[UComponent.uTypeKey].ToString();
                var uComponent = GetUComponent(uComponentToken,Type.GetType(typeDefintion));
                uComponents.Add(uComponent);
            }

            return uComponents;
        }

        public static UComponent GetUComponent(JToken uComponentToken, Type type)
        {   Debug.Log(type);
            // TODO: Eventually I'll need to make this accept generic component children with expandable classes. [M]
            if(type == typeof(UTransform))
            {
                var uTransform = new UTransform();
                
                uTransform.position = new Vector3(float.Parse(uComponentToken[UTransform.positionKey]["u_x"].ToString()), float.Parse(uComponentToken[UTransform.positionKey]["u_y"].ToString()), float.Parse(uComponentToken[UTransform.positionKey]["u_z"].ToString()));
                uTransform.rotation = new Vector3(float.Parse(uComponentToken["u_rotation"]["u_x"].ToString()), float.Parse(uComponentToken["u_rotation"]["u_y"].ToString()), float.Parse(uComponentToken["u_rotation"]["u_z"].ToString()));
                uTransform.scale = new Vector3(float.Parse(uComponentToken["u_scale"]["u_x"].ToString()), float.Parse(uComponentToken["u_scale"]["u_y"].ToString()), float.Parse(uComponentToken["u_scale"]["u_z"].ToString()));

                uTransform.parentName = uComponentToken[UTransform.parentNameKey].ToString();
                return uTransform;
            }
            else if(type == typeof(UMeshFilter))
            {
                var uMeshFilter = new UMeshFilter();
                uMeshFilter.meshName = uComponentToken[UMeshFilter.meshNameKey].ToString();
                return uMeshFilter;
            }
            return default;
        }
    }
}