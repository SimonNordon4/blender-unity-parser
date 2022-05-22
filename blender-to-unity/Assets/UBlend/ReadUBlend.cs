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
    public class ReadUBlend
    {
        public const string k_type = "type";
        public const string name_space = "UBlend.";

        public static JObject GetJObject(string json)
        {
            return JObject.Parse(json);
        }

        // Start digging down into the json object
        public static Data GetUBlend(JObject json)
        {
            Data data = new Data();

            GetAssets(json[nameof(data.u_assets)], data.u_assets);
            GetObjects(json[nameof(data.u_objects)], data.u_objects);

            return data;
        }

        // grab our assets
        public static void GetAssets(JToken u_assetsToken, UAssets u_assets)
        {

            u_assets = u_assetsToken.ToObject<UAssets>();
            //get meshes
            //get materials
            //get textures
        }

        // We are living a get objects level incase there are other instances that aren't gameobjects.
        public static void GetObjects(JToken u_objectsToken, UObjects u_objects)
        {
            GetGameObjects(u_objectsToken[nameof(u_objects.u_gameobjects)], u_objects.u_gameobjects);
        }

        // Get our gameobjects, assign a name and components.
        public static void GetGameObjects(JToken u_gameobjectsToken, List<UGameObject> u_gameobjects)
        {
            foreach (JToken t in u_gameobjectsToken)
            {
                var go = new UGameObject();
                go.name = t[nameof(go.name)].ToString();
                SetUTransform(t[nameof(go.u_transform)], go.u_transform);
                SetComponents(t[nameof(go.u_components)], go.u_components);
                u_gameobjects.Add(go);
            }
        }

        public static void SetUTransform(JToken u_transformToken, UTransform u_transform)
        {
            u_transform.parent_name = u_transformToken[nameof(u_transform.parent_name)].ToString();
            float[] pos = u_transformToken[nameof(u_transform.position)].ToObject<float[]>();
            u_transform.position = new Vector3(pos[0], pos[1], pos[2]);

            float[] rot = u_transformToken[nameof(u_transform.rotation)].ToObject<float[]>();
            u_transform.rotation = new Vector3(rot[0], rot[1], rot[2]);

            float[] scale = u_transformToken[nameof(u_transform.scale)].ToObject<float[]>();
            u_transform.scale = new Vector3(scale[0], scale[1], scale[2]);
        }

        // Sort our components by their type.
        public static void SetComponents(JToken u_componentsToken, List<UComponent> u_components)
        {
            foreach (JToken t in u_componentsToken)
            {
                if((t[k_type].ToString()) == nameof(UMeshFilter))
                {
                    GetUMeshFilter(t, u_components);
                }
            }
        }

        public static void GetUMeshFilter(JToken u_meshFilterToken, List<UComponent> u_components)
        {
            UMeshFilter uMeshFilter = new UMeshFilter();
            uMeshFilter.mesh_name = u_meshFilterToken[nameof(uMeshFilter.mesh_name)].ToString();
            u_components.Add(uMeshFilter);
        }
    }



}