using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

namespace Blender.Importer.Operators
{
    public class BlendMeshesDeserializer
    {
        public BlendMeshesDeserializer(string dataPath)
        {
            this.DeserializeData(dataPath);
        }
        private BlendMeshes _blendMeshes;
        private Dictionary<string, Mesh> _meshIdMap;

        public BlendMeshes GetBlendMeshes()
        {
            if (_blendMeshes == null)
            {
                throw new System.Exception("BlendMeshes is null");
            }
            return this._blendMeshes;
        }

        public Dictionary<string, Mesh> GetMeshIdMap()
        {
            if (_meshIdMap == null)
            {
                throw new System.Exception("MeshIdMap is null");
            }
            return this._meshIdMap;
        }

        private void DeserializeData(string dataPath)
        {
            // Check if path is valid.
            if(!File.Exists(dataPath))
            {
                throw new System.Exception("File does not exist");
            }

            var json = File.ReadAllText(dataPath);
            _blendMeshes = JsonUtility.FromJson<BlendMeshes>(json);

            // If the blendmesh is valid data, generate meshes.
            if (ValidateData())
            {
                GenerateMeshes();
            }
            else
            {
                _meshIdMap = new Dictionary<string, Mesh>();
            }
        }

        // TODO. Each mesh must have a name_id, 1 submesh, and some vertices. Mod 3 and 2 the vector 3s and 2s.
        private bool ValidateData()
        {
            if (_blendMeshes == null)
            {
                f.printError("BlendMeshes is null", "Deserialization");
                return false;
            }
            else
            {
                return true;
            }

        }

        private void GenerateMeshes()
        {
            foreach (var blendMesh in _blendMeshes.meshes)
            {
                Mesh mesh = new Mesh();
                mesh.name = blendMesh.name_id;

                // Vertices
                mesh.SetVertices(FloatToVector3(blendMesh.vertices));

                // Normals
                if (blendMesh.normals.Length > 0)
                    mesh.SetNormals(FloatToVector3(blendMesh.normals));
                else
                    mesh.RecalculateNormals();

                // UVs
                if (blendMesh.uv.Length > 0)
                    mesh.SetUVs(0, FloatToVector2(blendMesh.uv));
                if (blendMesh.uv2.Length > 0)
                    mesh.SetUVs(1, FloatToVector2(blendMesh.uv2));
                if (blendMesh.uv3.Length > 0)
                    mesh.SetUVs(2, FloatToVector2(blendMesh.uv3));
                if (blendMesh.uv4.Length > 0)
                    mesh.SetUVs(3, FloatToVector2(blendMesh.uv4));
                if (blendMesh.uv5.Length > 0)
                    mesh.SetUVs(4, FloatToVector2(blendMesh.uv5));
                if (blendMesh.uv6.Length > 0)
                    mesh.SetUVs(5, FloatToVector2(blendMesh.uv6));
                if (blendMesh.uv7.Length > 0)
                    mesh.SetUVs(6, FloatToVector2(blendMesh.uv7));
                if (blendMesh.uv8.Length > 0)
                    mesh.SetUVs(7, FloatToVector2(blendMesh.uv8));

                // Triangles
                mesh.subMeshCount = blendMesh.sub_meshes.Length;
                for (int i = 0; i < blendMesh.sub_meshes.Length; i++)
                {
                    mesh.SetTriangles(blendMesh.sub_meshes[i].triangles, i);
                }

                // Finished
                _meshIdMap.Add(blendMesh.name_id, mesh);
            }
        }

        private Vector3[] FloatToVector3(float[] floats)
        {
            var vectors = new Vector3[floats.Length / 3];
            for (int i = 0; i < vectors.Length; i++)
            {
                vectors[i] = new Vector3(floats[i * 3], floats[i * 3 + 1], floats[i * 3 + 2]);
            }
            return vectors;
        }

        private Vector2[] FloatToVector2(float[] floats)
        {
            var vectors = new Vector2[floats.Length / 2];
            for (int i = 0; i < vectors.Length; i++)
            {
                vectors[i] = new Vector2(floats[i * 2], floats[i * 2 + 1]);
            }
            return vectors;
        }
    }
}