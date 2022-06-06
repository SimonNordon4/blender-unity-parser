using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Blender.Importer
{
    public class MeshCreator
    {
        public static Mesh CreateMesh(BlendMesh blendMesh)
        {
            var mesh = new Mesh();
            mesh.SetVertices(FloatToVector3(blendMesh.vertices));
            mesh.SetNormals(FloatToVector3(blendMesh.normals));
            mesh.subMeshCount = blendMesh.sub_meshes.Length;
            for (int i = 0; i < blendMesh.sub_meshes.Length; i++)
            {
                mesh.SetTriangles(blendMesh.sub_meshes[i].triangles, i);
            }
            return mesh;
        }

        private static Vector3[] FloatToVector3(float[] floats)
        {
            var vectors = new Vector3[floats.Length / 3];
            for (int i = 0; i < vectors.Length; i++)
            {
                vectors[i] = new Vector3(floats[i * 3], floats[i * 3 + 1], floats[i * 3 + 2]);
            }
            return vectors;
        }

        private static Vector2[] FloatToVector2(float[] floats)
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