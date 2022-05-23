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
    public class UBlendJsonNet_JObject
    {
        public const string k_type = "type";
        public const string name_space = "UBlend.";

        public static JObject GetJObject(string json)
        {
            return JObject.Parse(json);
        }

        // Start digging down into the json object
        public static Data GetUBlend(string json)
        {
            Data data = new Data();
            JObject jObject = GetJObject(json);

            SetAssets(jObject[nameof(data.u_assets)], data.u_assets);
            SetObjects(jObject[nameof(data.u_objects)], data.u_objects);

            return data;
        }

        // grab our assets
        public static void SetAssets(JToken u_assetsToken, UAssets u_assets)
        {

            SetMeshes(u_assetsToken[nameof(u_assets.u_meshes)], u_assets.u_meshes);
            //get meshes
            //get materials
            //get textures
        }

        public static void SetMeshes(JToken u_meshesToken, List<UMesh> u_meshes)
        {
            foreach (var t in u_meshesToken)
            {
                var u_mesh = new UMesh();
                u_mesh.u_name = t[nameof(u_mesh.u_name)].ToString();
                u_mesh.u_vertices = GetVector3List(t[nameof(u_mesh.u_vertices)]);
                u_mesh.u_normals = GetVector3List(t[nameof(u_mesh.u_normals)]);
                
                foreach (JToken uvToken in t[nameof(u_mesh.u_uvs)])
                {
                    u_mesh.u_uvs.Add(new UUVLayer(GetVector2List(uvToken)));
                }
                u_mesh.u_submesh_count = (int)t[nameof(u_mesh.u_submesh_count)];

                foreach(JToken tris in t[nameof(u_mesh.u_submesh_triangles)])
                {
                    u_mesh.u_submesh_triangles.Add(new SubMeshTriangles() { u_triangles = GetIntList(tris) });
                }
                u_meshes.Add(u_mesh);
            }
        }

        // We are living a get objects level incase there are other instances that aren't gameobjects.
        public static void SetObjects(JToken u_objectsToken, UObjects u_objects)
        {
            SetGameObjects(u_objectsToken[nameof(u_objects.u_gameobjects)], u_objects.u_gameobjects);
        }

        // Get our gameobjects, assign a name and components.
        public static void SetGameObjects(JToken u_gameobjectsToken, List<UGameObject> u_gameobjects)
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

        #region Utility
        public static List<Vector3> GetVector3List(JToken token)
        {
            List<Vector3> list = new List<Vector3>();
            foreach (JToken t in token)
            {
                float[] vec3 = t.ToObject<float[]>();
                list.Add(new Vector3(vec3[0], vec3[1], vec3[2]));  
            }
            return list;
        }

        public static List<Vector2> GetVector2List(JToken token)
        {
            List<Vector2> list = new List<Vector2>();
            foreach (JToken t in token)
            {
                float[] pos = t.ToObject<float[]>();
                list.Add(new Vector2(pos[0], pos[1]));
            }
            return list;
        }

        public static List<int> GetIntList(JToken token)
        {
            List<int> list = new List<int>();
            foreach (JToken t in token)
            {
                list.Add((int)t);
            }
            return list;
        }
        #endregion
    }



}